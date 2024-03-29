using AutoMapper;
using BMOS.BAL.Helpers;
using Firebase.Auth;
using GenZStyleApp.DAL.DAO;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
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
    public class AccountRepository : IAccountRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IEmailRepository _emailRepository;

        public AccountRepository(IUnitOfWork unitOfWork, IMapper mapper, IEmailRepository emailRepository)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
            _emailRepository = emailRepository;
        }

        public async Task<List<GetAccountResponse>> GetAccountssAsync()
        {
            try
            {
                List<Account> accounts = await this._unitOfWork.AccountDAO.GetAllAccount();
                return this._mapper.Map<List<GetAccountResponse>>(accounts);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        public async Task ChangPassword(int accountId, ChangePasswordRequest changPasswordRequest)
        {    //phu1234
            //nam1234
            //Admin@123
            try
            {   
                var account = await _unitOfWork.AccountDAO.GetAccountById(accountId);
                if (account == null)
                {
                    throw new NotFoundException("AccountId does not exist in system.");
                }
                // Kiểm tra xem mật khẩu cũ có khớp với mật khẩu hiện tại không
                /*bool isPasswordValid = BCrypt.Net.BCrypt.Verify(changPasswordRequest.OldPassword, account.PasswordHash);
                if (!isPasswordValid)
                {
                    throw new BadRequestException("Old password does not match with current password.");
                }*/

                if (changPasswordRequest.NewPassword != changPasswordRequest.ConfirmPassword)
                {
                    throw new BadRequestException("New password and old password do not match each other.");
                }

                // Mã hóa mật khẩu mới
                /*string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(changPasswordRequest.NewPassword);*/
                // Cập nhật mật khẩu đã được mã hóa vào tài khoản
                account.PasswordHash = changPasswordRequest.NewPassword;
                _unitOfWork.AccountDAO.ChangePassword(account);
                _unitOfWork.Commit();
                /*return _mapper.Map<GetAccountResponse>(account);*/
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

        public async Task<List<GetAccountResponse>> SearchByUserName(string username)
        {
            try
            {
                
                // Sử dụng hàm SearchByUsername từ AccountDAO
                List<Account> accounts = await _unitOfWork.AccountDAO.SearchByUsername(username);

                // Chuyển đổi List<Account> thành List<AccountDTO> nếu cần thiết
                List<GetAccountResponse> accountDTOs = _mapper.Map<List<GetAccountResponse>>(accounts);

                return accountDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Account> FindAccountByEmail(string email)
        {
            try
            {
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(email);
                return account;
            }catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        /*public async Task<List<GetAccountResponse>> GetSuggestionUsersAsync(HttpContext httpContext)
        {
            JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
            string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
            var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);


            var followings = await this._unitOfWork.userRelationDAO.GetFollowing(account.AccountId);//người này đang follow bao nhiêu người                       
            var listaccount = await _unitOfWork.AccountDAO.GetSuggestionAccount(followings);

            var Listaccounts = _mapper.Map<List<GetAccountResponse>>(listaccount); 
            List<GetAccountResponse> result = new List<GetAccountResponse>();
            foreach (var accountDTO in Listaccounts)
            {
                var follower = await this._unitOfWork.userRelationDAO.GetFollower(accountDTO.AccountId);// người này có bao nhiêu follower
                GetAccountResponse getAccountResponses = new GetAccountResponse()
                {
                    AccountId = accountDTO.AccountId,
                    Email = "null", // Đảm bảo là null, không phải chuỗi rỗng
                    Firstname = "null",
                    Lastname = "null",
                    PasswordHash = "null",
                    AvatarUrl = "null",
                    Gender = accountDTO.Gender,
                    Username = accountDTO.Username,
                    Height = accountDTO.User.Height,
                    follower = follower.Count(),
                    User = accountDTO.User,
                    Posts = accountDTO.Posts,
                };
                result.Add(getAccountResponses);
            }
            
            
            *//*return _mapper.Map<List<GetAccountResponse>>(listaccount);*//*
            return result;

        }*/

        public async Task<List<GetAccountResponse>> GetSuggestionUsersAsync(HttpContext httpContext)
        {
            JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
            string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
            var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

            
            var followings = await this._unitOfWork.userRelationDAO.GetFollowing(account.AccountId);//người này đang follow bao nhiêu người                       
            var listaccount = await _unitOfWork.AccountDAO.GetSuggestionAccount(followings, account.AccountId);

            var Listaccounts = _mapper.Map<List<GetAccountResponse>>(listaccount);
            List<GetAccountResponse> result1 = new List<GetAccountResponse>();//khoi tao 1 cai list
            List<GetAccountResponse> result2 = new List<GetAccountResponse>();//khoi tao 1 cai list
            List<GetAccountResponse> Totalresult = new List<GetAccountResponse>();//khoi tao 1 cai list
            foreach (var accountDTO in Listaccounts)
            {
                var follower = await this._unitOfWork.userRelationDAO.GetFollower(accountDTO.AccountId);// người này có bao nhiêu follower
                GetAccountResponse getAccountResponses = new GetAccountResponse()
                {
                    AccountId = accountDTO.AccountId,
                    Email = "null", // Đảm bảo là null, không phải chuỗi rỗng
                    Firstname = "null",
                    Lastname = "null",
                    PasswordHash = "null",
                    AvatarUrl = "null",
                    isfollow = false,
                    Gender = accountDTO.Gender,
                    Username = accountDTO.Username,
                    Height = accountDTO.User.Height,
                    follower = follower.Count(),
                    User = accountDTO.User,
                    Posts = accountDTO.Posts,
                };
                result1.Add(getAccountResponses);// trả về những người chưa follow 
            }
            var listrestaccount = await _unitOfWork.AccountDAO.GetListRestAccount(listaccount, account.AccountId);
            var listrestaccounts = _mapper.Map<List<GetAccountResponse>>(listrestaccount);
            foreach (var accountDTOO in listrestaccounts)
            {
                var follower = await this._unitOfWork.userRelationDAO.GetFollower(accountDTOO.AccountId);// người này có bao nhiêu follower
                GetAccountResponse getAccountResponsess = new GetAccountResponse()
                {
                    AccountId = accountDTOO.AccountId,
                    Email = "null", // Đảm bảo là null, không phải chuỗi rỗng
                    Firstname = "null",
                    Lastname = "null",
                    PasswordHash = "null",
                    AvatarUrl = "null",
                    isfollow = true,
                    Gender = accountDTOO.Gender,
                    Username = accountDTOO.Username,
                    Height = accountDTOO.User.Height,
                    follower = follower.Count(),
                    User = accountDTOO.User,
                    Posts = accountDTOO.Posts,
                };
                result2.Add(getAccountResponsess);// trả về những người đã follow 
            }

            /*return _mapper.Map<List<GetAccountResponse>>(listaccount);*/
            Totalresult.AddRange(result2);
            Totalresult.AddRange(result1);
            return Totalresult;

        }
        public async Task<GetAccountResponse> GetSuggestionUsersIdAsync(int accountId, HttpContext httpContext)
        {
            JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
            string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
            var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

            var account1 = await _unitOfWork.AccountDAO.GetAccountById(accountId);

            var followings = await this._unitOfWork.userRelationDAO.GetFollowing(account.AccountId);//người này đang follow bao nhiêu người

            bool isFollowing = followings.Any(following => following.FollowingId == account1.AccountId);
            var accounts = _mapper.Map<GetAccountResponse>(account1);
            GetAccountResponse getAccountResponsess;
            if (isFollowing == true)
            {
                var follower = await this._unitOfWork.userRelationDAO.GetFollower(accountId);// người này có bao nhiêu follower
                var following = await this._unitOfWork.userRelationDAO.GetFollowing(accountId);// người này có bao nhiêu following
                 getAccountResponsess = new GetAccountResponse()
                {
                    AccountId = accounts.AccountId,
                    Email = "null", // Đảm bảo là null, không phải chuỗi rỗng
                    Firstname = "null",
                    Lastname = "null",
                    PasswordHash = "null",
                    AvatarUrl = "null",
                    isfollow = true,
                    Gender = accounts.Gender,
                    Username = accounts.Username,
                    Height = accounts.User.Height,
                    follower = follower.Count(),
                    following = following.Count(),
                    User = accounts.User,
                    Posts = accounts.Posts,
                };
            }
            else // isFollowing == false
            {
                var follower = await this._unitOfWork.userRelationDAO.GetFollower(accountId);
                var following = await this._unitOfWork.userRelationDAO.GetFollowing(accountId);
                 getAccountResponsess = new GetAccountResponse()
                {
                    AccountId = accounts.AccountId,
                    Email = "null",
                    Firstname = "null",
                    Lastname = "null",
                    PasswordHash = "null",
                    AvatarUrl = "null",
                    isfollow = false, // Đổi thành false
                    Gender = accounts.Gender,
                    Username = accounts.Username,
                    Height = accounts.User.Height,
                    follower = follower.Count(),
                    following = following.Count(),
                    User = accounts.User,
                    Posts = accounts.Posts,
                };


            }
            return getAccountResponsess;
        }
        public async Task ResetPassword(string email)
        {
            try
            {
                 var account = await _unitOfWork.AccountDAO.GetAccountByEmail(email);
                if(account == null)
                {
                    throw new NotFoundException("Account does not exist in system.");
                }
                
                var password = Guid.NewGuid().ToString().Substring(0,10);
                string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(password);
                account.PasswordHash = hashedNewPassword;
                await _unitOfWork.AccountDAO.UpdateAccountProfile(account);
                await _unitOfWork.CommitAsync();
                var message = new GenZStyleAPP.BAL.Models.Message(new string[] { account.Email! }, "Reset old password to newpassword", password!);
                /*var confirmationLink = Url.Action(nameof(ConfirmEmail), "Users", new { token, email = user.Email }, Request.Scheme);
                var message = new GenZStyleAPP.BAL.Models.Message(new string[] { user.Email! }, "Confirmation email link", confirmationLink!);*/
                _emailRepository.SendEmail(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
