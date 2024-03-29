using AutoMapper;
using BMOS.DAL.Enums;
using GenZStyleApp.BAL.Helpers;
using GenZStyleApp.DAL.Enums;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Package;
using GenZStyleAPP.BAL.Exceptions;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class PackageRepository : IPackageRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PackageRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        #region GetAllPackage
        public async Task<List<GetPackageResponse>> GetPackagesAsync()
        {
            try
            {
                var packages = await _unitOfWork.packageDAO.GetAllPackages();
                return _mapper.Map<List<GetPackageResponse>>(packages);
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
        }
        #endregion
        public async Task<GetPackageResponse> PurcharePackage(int PackageId, HttpContext httpContext)
        {
            try
            {   var role = await _unitOfWork.RoleDAO.GetRoleAsync((int)RoleEnum.Role.KOL);
                var Packages = await _unitOfWork.packageDAO.GetPackageByIdAsync(PackageId);
                if (Packages == null)
                {
                    throw new NotFoundException("Package does not exist");
                }
                // Example: Check if the package is available for registration
                
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                var user = await _unitOfWork.UserDAO.GetUserByEmailAsync(emailFromClaim);

                var registrationFee = Packages.Cost; 

                
                

                // Update role 
                user.Role = role; // Change to the new role
                _unitOfWork.UserDAO.UpdateUser(user);
                // Lưu thông tin đăng ký gói dịch vụ vào cơ sở dữ liệu
                var packageRegistration = new PackageRegistration
                {   
                    AccountId = account.AccountId,
                    PackageId = PackageId,
                    RegistrationDate = DateTime.Now,
                    Account = account,
                    Package = Packages
                    // Các thông tin khác của đăng ký gói dịch vụ nếu cần
                };
                // Tạo giao dịch ví cho đăng ký gói dịch vụ
                var registrationTransaction = new Invoice
                {
                    AccountId = account.AccountId,
                    PackageId = PackageId,
                    RechargeID = DateTime.Now.Ticks.ToString(),
                    Date = DateTime.Now,
                    Total = registrationFee,
                    //Content = $"Package registration for {Package.PackageName}",
                    PaymentType = TransactionEnum.TransactionType.SEND.ToString(),
                    Status = (int)TransactionEnum.RechangeStatus.SUCCESSED,
                };
                // Thực hiện ghi log và cập nhật số dư ví
                await _unitOfWork.InvoiceDAO.CreateInvoiceAsync(registrationTransaction);
                /*Packages.PackageRegistrations.Add(packageRegistration);*/

                await _unitOfWork.PackageRegistrationDAO.AddNewPackageRegistration(packageRegistration);
                
                // Save and update changes in the database
                await _unitOfWork.CommitAsync();
                // Trả về thông tin gói dịch vụ sau khi đăng ký thành công
                return _mapper.Map<GetPackageResponse>(Packages);


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
            catch (PaymentFailedException ex)
            {
                // Handle payment failure
                throw new BadRequestException("Payment failed. Please try again or choose a different payment method.");
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
            

        }
        #region CreateNewPackageAsync
        public async Task<GetPackageResponse> CreateNewPackageAsync(GetPackageRequest addPackageRequest, FireBaseImage fireBaseImage, HttpContext httpContext)
        {
            try
            {
                //var images = new List<Post>();
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                Package package = this._mapper.Map<Package>(addPackageRequest);



                package.PackageName = addPackageRequest.PackageName;
                package.Cost = addPackageRequest.Cost;
                package.Description = addPackageRequest.Description;
                package.Status = "Active";
                package.IsStatus = 0;


                #region Upload images to firebase

                //foreach (var imageFile in addPostRequest.Image)
                //{
                FileHelper.SetCredentials(fireBaseImage);
                FileStream fileStream = FileHelper.ConvertFormFileToStream(addPackageRequest.Image);
                Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "Package");

                // Assuming you want to store multiple image URLs
                // Consider creating a separate entity for images if necessary
                // and establish a one-to-many relationship with the Post entity
                // For now, appending URLs to the Image property
                package.Image = result.Item1; // Separate URLs by a delimiter
                                              //}

                #endregion
                await _unitOfWork.packageDAO.AddNewPackage(package);
                await this._unitOfWork.CommitAsync();


                return this._mapper.Map<GetPackageResponse>(package);

            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }

        }
        #endregion
        #region UpdatePackageByIdAsync
        public async Task<GetPackageResponse> UpdatePackageByIdAsync(int packageId,
                                                                                     FireBaseImage fireBaseImage,
                                                                                     UpdatePackageRequest updatePackageRequest, HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                Package package = await _unitOfWork.packageDAO.GetPackageByIdAsync(packageId);

                if (package == null)
                {
                    throw new NotFoundException("PackageId does not exist in system");
                }

                package.PackageName = updatePackageRequest.PackageName;
                package.Cost = updatePackageRequest.Cost;
                package.Description = updatePackageRequest.Description;
                package.Status = "Update";
                package.IsStatus = 1;


                //if (updateCustomerRequest.PasswordHash != null)
                //{
                //    customer.Account.PasswordHash = StringHelper.EncryptData(updateCustomerRequest.PasswordHash);
                //}

                #region Upload image to firebase
                if (updatePackageRequest.Image != null)
                {
                    FileHelper.SetCredentials(fireBaseImage);
                    //await FileHelper.DeleteImageAsync(user.AvatarUrl, "User");
                    FileStream fileStream = FileHelper.ConvertFormFileToStream(updatePackageRequest.Image);
                    Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "Package");
                    package.Image = result.Item1;
                    //customer.AvatarID = result.Item2;
                }
                #endregion

                _unitOfWork.packageDAO.UpdatePackage(package);
                await this._unitOfWork.CommitAsync();
                return _mapper.Map<GetPackageResponse>(package);
            }

            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("PackageId", ex.Message);
                throw new NotFoundException(error);
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
