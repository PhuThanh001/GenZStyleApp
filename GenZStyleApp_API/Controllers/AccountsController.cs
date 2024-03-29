using FluentValidation;
using FluentValidation.Results;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    
    public class AccountsController : ODataController
    {
        private IAccountRepository _accountRepository;
        private IValidator<ChangePasswordRequest> _changePasswordValidator;


        public AccountsController(IAccountRepository accountRepository,
            IValidator<ChangePasswordRequest> changePasswordValidator)
        {
            _accountRepository = accountRepository;
            _changePasswordValidator = changePasswordValidator;
        }

        [EnableQuery]
        [HttpPut("odata/Accounts/{key}/Update")]
        public async Task<IActionResult> Put([FromRoute] int key, [FromBody] ChangePasswordRequest changePasswordRequest)
        {
            ValidationResult validationResult = await _changePasswordValidator.ValidateAsync(changePasswordRequest);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }
            /*GetAccountResponse result = await this._accountRepository.ChangPassword(key, changePasswordRequest);*/
            await this._accountRepository.ChangPassword(key, changePasswordRequest);
            return Ok();
            /*return Updated(result);*/
        }

        [EnableQuery]
        [HttpPut("odata/Accounts/ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try {
                
                await this._accountRepository.ResetPassword(email);
                return Ok(); 
            
            }
            catch (Exception ex) 
            {
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
             
            
        }

        #region SearchByUserName
        [HttpGet("odata/Accounts/SearchByUserName")]
        [EnableQuery]
        public async Task<IActionResult> SearchByUserName([FromQuery] string username)
        {
            try
            {
                List<GetAccountResponse> accountDTOs = await _accountRepository.SearchByUserName(username);
                List<GetAccountResponse> accountDTOss = new List<GetAccountResponse>();
                // Nếu muốn thực hiện bất kỳ xử lý hoặc kiểm tra nào đó trước khi trả kết quả, bạn có thể thêm vào đây

                if (accountDTOs.Count > 0)
                {
                    // Thành công, trả về thông báo thành công và danh sách tài khoản
                    return Ok(new { Message = "Tìm kiếm thành công.", Accounts = accountDTOs });
                }
                else
                {
                    // Không tìm thấy tài khoản, trả về thông báo không có kết quả
                    return Ok(new { Message = "Không tìm thấy tài khoản nào.", Accounts = accountDTOss });
                }
            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
                     
        }
        #endregion
        #region GetSuggestionAccount
        [HttpGet("odata/Accounts/GetSuggestionAccount")]
        [EnableQuery]

        public async Task<IActionResult> GetSuggestionAccount()
        {
            try
            {
                List<GetAccountResponse> Accounts = await _accountRepository.GetSuggestionUsersAsync(HttpContext);
                return Ok(Accounts);
                    
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }


        }

        #endregion
        #region GetSuggestionAccount
        [HttpGet("odata/Accounts/GetSuggestionAccountId")]
        [EnableQuery]

        public async Task<IActionResult> GetSuggestionAccountId(int accountId)
        {
            try
            {
                GetAccountResponse Accounts = await _accountRepository.GetSuggestionUsersIdAsync(accountId, HttpContext) ;
                return Ok(Accounts);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }


        }

        #endregion
        [HttpGet("odata/Accounts/CountAllAccount")]
        [EnableQuery]
        public async Task<IActionResult> CountAllAccount()
        {
            try
            {
                List<GetAccountResponse> account = await this._accountRepository.GetAccountssAsync();
                int totalAccount = account.Count;
                return Ok(new
                {
                    TotalUsers = totalAccount,

                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
