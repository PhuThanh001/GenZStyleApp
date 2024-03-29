using AutoMapper;
using BMOS.DAL.Enums;
using GenZStyleApp.DAL.Enums;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Invoices;
using GenZStyleAPP.BAL.DTOs.Transactions;
using GenZStyleAPP.BAL.DTOs.Transactions.MoMo;
using GenZStyleAPP.BAL.DTOs.Transactions.ZaloPay;
using GenZStyleAPP.BAL.Heplers;
using GenZStyleAPP.BAL.Heplers.ZaloPayHelper;
using GenZStyleAPP.BAL.Heplers.ZaloPayHelper.Crypto;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenZStyleAPP.BAL.Heplers.ZaloPayHelper.Crypto.HmacHelper;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class TransactionRepository : ITransactionRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public TransactionRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        #region Create wallet transaction
        public async Task<GetTransactionResponse> CreateWalletTransactionAsync(PostTransactionRequest model,int packageId, MomoConfigModel _config)
        {
            try
            {
                var role = await _unitOfWork.RoleDAO.GetRoleAsync((int)RoleEnum.Role.KOL);
                var Packages = await _unitOfWork.packageDAO.GetPackageByIdAsync(packageId);
                if (Packages == null)
                {
                    throw new NotFoundException("Package does not exist");
                }
                // Example: Check if the package is available for registration
                #region Validation           
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(model.Email);
                if (account == null)
                {
                    throw new NotFoundException("Account does not exist.");
                }
                #endregion
                
                // Lưu thông tin đăng ký gói dịch vụ vào cơ sở dữ liệu
                var packageRegistration = new PackageRegistration
                {
                    AccountId = account.AccountId,
                    PackageId = packageId,
                    RegistrationDate = DateTime.Now,
                    Account = account,
                    Package = Packages
                    // Các thông tin khác của đăng ký gói dịch vụ nếu cần
                };
                #region Add transaction to Db (Status: Pending)
                // Must save to database to check Amount and [Currency = false] when there is a notification from Momo
                var orderId = DateTime.Now.Ticks.ToString();
                

                Invoice invoice = new Invoice
                {   
                    AccountId = account.AccountId,
                    PackageId = packageId,
                    RechargeID = orderId,
                    Date = DateTime.Now,
                    Total = model.Amount,
                    PaymentType = TransactionEnum.TransactionType.DEPOSIT.ToString(),
                    Status = (int)TransactionEnum.RechangeStatus.PENDING,
                    Account = account,
                    
                };

                await _unitOfWork.InvoiceDAO.CreateInvoiceAsync(invoice);
                await _unitOfWork.PackageRegistrationDAO.AddNewPackageRegistration(packageRegistration);
                await _unitOfWork.CommitAsync();
                #endregion

                #region Send request to momo
                var requestId = orderId + "id";
                var rawData = $"accessKey={_config.AccessKey}&amount={model.Amount}&extraData={_config.ExtraData}&ipnUrl={_config.NotifyUrl}&orderId={orderId}&orderInfo={_config.OrderInfo}&partnerCode={_config.PartnerCode}&redirectUrl={model.RedirectUrl}&requestId={requestId}&requestType={_config.RequestType}";
                var signature = EncodeHelper.ComputeHmacSha256(rawData, _config.SecretKey!);

                var client = new RestClient(_config.PayGate! + "/create");
                var request = new RestRequest() { Method = Method.Post };
                request.AddHeader("Content-Type", "application/json; charset=UTF-8");

                // Body of request
                PostTransactionMomoRequest bodyContent = new PostTransactionMomoRequest
                {
                    partnerCode = _config.PartnerCode,
                    partnerName = _config.PartnerName,
                    storeId = _config.PartnerCode,
                    requestType = _config.RequestType,
                    ipnUrl = _config.NotifyUrl,
                    redirectUrl = model.RedirectUrl,
                    orderId = orderId,
                    amount = model.Amount,
                    lang = _config.Lang,
                    autoCapture = _config.AutoCapture,
                    orderInfo = _config.OrderInfo,
                    requestId = requestId,
                    extraData = _config.ExtraData,
                    orderExpireTime = _config.OrderExpireTime,
                    signature = signature
                };

                request.AddParameter("application/json", JsonConvert.SerializeObject(bodyContent), ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var responseContent = JsonConvert.DeserializeObject<PostTransactionMomoResponse>(response.Content!);
                    var walletResponse = _mapper.Map<GetTransactionResponse>(invoice);
                    walletResponse.PayUrl = responseContent!.PayUrl;
                    walletResponse.Deeplink = responseContent!.Deeplink;
                    walletResponse.QrCodeUrl = responseContent!.QrCodeUrl;
                    walletResponse.Applink = responseContent.Applink;

                    return walletResponse;
                }

                throw new Exception("No server is available to handle this request.");

                #endregion

            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion
        #region Create zalo transaction
        public async Task<PostTransactionZaloResponse> CreateZaloTransactionAsync(PostTransactionRequest model, int packageId, ZaloConfigModel config)
        {
            var role = await _unitOfWork.RoleDAO.GetRoleAsync((int)RoleEnum.Role.KOL);
            var Packages = await _unitOfWork.packageDAO.GetPackageByIdAsync(packageId);
            if (Packages == null)
            {
                throw new NotFoundException("Package does not exist");
            }
            #region Validation
            var account = await _unitOfWork.AccountDAO.GetAccountByEmail(model.Email);
            if (account == null)
            {
                throw new NotFoundException("Account does not exist.");
            }
            #endregion
            // Lưu thông tin đăng ký gói dịch vụ vào cơ sở dữ liệu
            var packageRegistration = new PackageRegistration
            {
                AccountId = account.AccountId,
                PackageId = packageId,
                RegistrationDate = DateTime.Now,
                Account = account,
                Package = Packages
                // Các thông tin khác của đăng ký gói dịch vụ nếu cần
            };
            #region Add transaction to Db (Status: Pending)
            // Must save to database to check Amount and [Currency = false] when there is a notification from Momo
            var orderId = DateTime.Now.Ticks.ToString();

            Invoice invoice = new Invoice
            {
                AccountId = account.AccountId,                
                PackageId = packageId,
                RechargeID = orderId,
                Date = DateTime.Now,
                Total = model.Amount,
                PaymentType = TransactionEnum.TransactionType.DEPOSIT.ToString(),
                Status = (int)TransactionEnum.RechangeStatus.SUCCESSED,
                Account = account,
                
            

        };
            invoice.Account.User.Role = role;
            await _unitOfWork.InvoiceDAO.CreateInvoiceAsync(invoice);
            await _unitOfWork.PackageRegistrationDAO.AddNewPackageRegistration(packageRegistration);
            await _unitOfWork.CommitAsync();
            #endregion
            #region Send request to Zalopay
            Random rnd = new Random();
            var embed_data = new Dictionary<string, string>(); // redirecturl here
            embed_data.Add("redirecturl", model.RedirectUrl);

            var items = new[] { new { } };
            var app_trans_id = rnd.Next(1000000); // Generate a random order's ID.
            var param = new Dictionary<string, string>();

            param.Add("app_id", config.app_id!);
            param.Add("app_user", config.app_user!);
            param.Add("app_time", Utils.GetTimeStamp().ToString());
            param.Add("amount", model.Amount.ToString());
            param.Add("app_trans_id", DateTime.Now.ToString("yyMMdd") + "_" + app_trans_id); // mã giao dich có định dạng yyMMdd_xxxx
            param.Add("embed_data", JsonConvert.SerializeObject(embed_data));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("description", "Lazada - Thanh toán đơn hàng #" + app_trans_id);
            param.Add("bank_code", config.bank_code!);

            var data = config.app_id! + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
              + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, config.key!, data));

            var result = await HttpHelper.PostFormAsync(config.url!, param);
            var response = PostTransactionZaloResponse.FromDictionary(result);

            return response;
            #endregion
        }
        #endregion
        #region Listen notification from Zalo
        public async Task<GetTransactionResponse> PaymentNotificationAsync(string id, ZaloConfigModel _config)
        {
            try
            {
                #region Validation

                var invoice = await _unitOfWork.InvoiceDAO.GetInvoiceByRechargeId(id);
                if (invoice == null)
                {
                    throw new NotFoundException("Transaction does not exist.");
                }

                if (invoice.Status == (int)TransactionEnum.RechangeStatus.SUCCESSED ||
                    invoice.Status == (int)TransactionEnum.RechangeStatus.FAILED)
                {
                    throw new BadRequestException("This transaction has been processed.");
                }

                #region Query transaction
                var requestId = invoice.RechargeID + "id";
                var rawData = $"app_id={_config.app_id}&key={_config.key}&url={_config.url}&app_user={_config.app_user}";
                var signature = EncodeHelper.ComputeHmacSha256(rawData, _config.key!);

                var client = new RestClient(_config.url! + "/query");
                var request = new RestRequest() { Method = Method.Post };
                request.AddHeader("Content-Type", "application/json; charset=UTF-8");

                QueryTransactionZaloRequest queryTransaction = new QueryTransactionZaloRequest
                {
                    app_id = _config.app_id,
                    key = _config.key,
                    url = _config.url,
                    app_user = _config.app_user,
                    description = _config.description,
                    bank_code = _config.bank_code,
                    signature = signature
                };

                request.AddParameter("application/json", JsonConvert.SerializeObject(queryTransaction), ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);

                var responseResult = JsonConvert.DeserializeObject<QueryTransactionZaloResponse>(response.Content!);
                #endregion


                // Check Amount and [Currency = fasle] 
                if (responseResult!.Amount != invoice.Total)
                {
                    throw new BadRequestException("Amount of transaction and notification does not matched!");
                }

                // Check legit of signature - coming soon
                #endregion

                #region Update wallettransaction and wallet (if success)
                // ResultCode = 0: giao dịch thành công
                // ResultCode = 9000: giao dịch được cấp quyền (authorization) thành công
                if (responseResult.ResultCode == 0 || responseResult.ResultCode == 9000)
                {
                    invoice.Status = (int)TransactionEnum.RechangeStatus.SUCCESSED;
                    _unitOfWork.InvoiceDAO.UpdateInvoice(invoice);

                    /*// If amount = null, amount = default value of type
                    invoice.Wallet.Balance += responseResult.Amount.GetValueOrDefault(0m);
                    _unitOfWork.WalletDAO.UpdateWallet(invoice.Wallet);
                    await _unitOfWork.CommitAsync();*/

                    return _mapper.Map<GetTransactionResponse>(invoice);
                }
                else if (responseResult.ResultCode == 1000)
                {
                    throw new BadRequestException("Transaction is initiated, waiting for user confirmation!");
                }
                else
                {
                    invoice.Status = (int)TransactionEnum.RechangeStatus.FAILED;
                    _unitOfWork.InvoiceDAO.UpdateInvoice(invoice);
                    await _unitOfWork.CommitAsync();

                    throw new BadRequestException("Recharge failed!");
                }
                #endregion
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (BadRequestException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new BadRequestException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion

        #region Listen notification from Momo
        public async Task<GetTransactionResponse> PaymentNotificationAsync(string id, MomoConfigModel _config)
        {
            try
            {
                #region Validation
                var role = await _unitOfWork.RoleDAO.GetRoleAsync((int)RoleEnum.Role.KOL);
                var invoice = await _unitOfWork.InvoiceDAO.GetInvoiceByRechargeId(id);
                if (invoice == null)
                {   

                    throw new NotFoundException("Transaction does not exist.");
                }

                if (invoice.Status == (int)TransactionEnum.RechangeStatus.SUCCESSED ||
                    invoice.Status == (int)TransactionEnum.RechangeStatus.FAILED)
                {
                                        
                    throw new BadRequestException("This transaction has been processed.");
                }

                #region Query transaction
                var requestId = invoice.RechargeID + "id";
                var rawData = $"accessKey={_config.AccessKey}&orderId={invoice.RechargeID}&partnerCode={_config.PartnerCode}&requestId={requestId}";
                var signature = EncodeHelper.ComputeHmacSha256(rawData, _config.SecretKey!);

                var client = new RestClient(_config.PayGate! + "/query");
                var request = new RestRequest() { Method = Method.Post };
                request.AddHeader("Content-Type", "application/json; charset=UTF-8");

                QueryTransactionMomoRequest queryTransaction = new QueryTransactionMomoRequest
                {
                    partnerCode = _config.PartnerCode,
                    requestId = requestId,
                    orderId = invoice.RechargeID,
                    lang = _config.Lang,
                    signature = signature

                };

                request.AddParameter("application/json", JsonConvert.SerializeObject(queryTransaction), ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);

                var responseResult = JsonConvert.DeserializeObject<QueryTransactionMomoResponse>(response.Content!);
                #endregion

                // Check Amount and [Currency = fasle] 
                if (responseResult!.Amount != invoice.Total)
                {
                    throw new BadRequestException("Amount of transaction and notification does not matched!");
                }

                // Check legit of signature - coming soon
                #endregion

                #region Update wallettransaction and wallet (if success)
                // ResultCode = 0: giao dịch thành công
                // ResultCode = 9000: giao dịch được cấp quyền (authorization) thành công
                if (responseResult.ResultCode == 0 || responseResult.ResultCode == 9000)
                {
                    invoice.Status = (int)TransactionEnum.RechangeStatus.SUCCESSED;
                    invoice.Account.User.Role = role;
                    await _unitOfWork.InvoiceDAO.UpdateInvoice(invoice);

                    // If amount = null, amount = default value of type
                    /*invoice.Wallet.Balance += responseResult.Amount.GetValueOrDefault(0m);
                    _unitOfWork.WalletDAO.UpdateWallet(invoice.Wallet);*/
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<GetTransactionResponse>(invoice);
                }
                else if (responseResult.ResultCode == 1000)
                {
                    throw new BadRequestException("Transaction is initiated, waiting for user confirmation!");
                }
                else
                {
                    invoice.Status = (int)TransactionEnum.RechangeStatus.FAILED;
                    _unitOfWork.InvoiceDAO.UpdateInvoice(invoice);
                    await _unitOfWork.CommitAsync();

                    throw new BadRequestException("Recharge failed!");
                }
                
                #endregion
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (BadRequestException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new BadRequestException(error);
                
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion
    }
}
