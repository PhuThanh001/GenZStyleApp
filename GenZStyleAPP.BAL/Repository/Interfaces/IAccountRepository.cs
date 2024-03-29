using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IAccountRepository
    {
        public Task ChangPassword(int accountId, ChangePasswordRequest changePasswordRequest);
        public Task<List<GetAccountResponse>> SearchByUserName(string username);

        public  Task<Account> FindAccountByEmail(string email);

        public Task<List<GetAccountResponse>> GetSuggestionUsersAsync(HttpContext httpContext);

        public Task<GetAccountResponse> GetSuggestionUsersIdAsync(int accountId, HttpContext httpContext);


        public Task<List<GetAccountResponse>> GetAccountssAsync();
        public Task ResetPassword(string email);
    }
}
