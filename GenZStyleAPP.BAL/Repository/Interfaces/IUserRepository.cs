using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.DTOs.UserRelations;
using GenZStyleAPP.BAL.DTOs.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.Accounts;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IUserRepository
    {
      
        public Task DeleteUserAsync(int id, HttpContext httpContext);
        public Task<List<GetUserResponse>> GetUsersAsync();
        public Task<GetUserResponse> GetActiveUser(int userId);
        public Task<GetUserResponse> UpdateUserProfileByAccountIdAsync(int accountId,
                                                                                     FireBaseImage fireBaseImage,
                                                                                     UpdateUserRequest updateUserRequest);
        public Task<GetUserResponse> OpenBanUserAsync(int accountId);
        public Task<GetUserResponse> GetUserByAccountIdAsync(int accountId);
        public Task<GetUserResponse> Register(FireBaseImage fireBaseImage,RegisterRequest registerRequest);

        public Task<GetUserRelationResponse> FollowUser(int AccountId, HttpContext httpContext);
        public Task<GetFollowResponse> GetFollowByProfileIdAsync(HttpContext httpContext);
        public Task<GetFollowResponse> GetFollowByAccountIdAsync(int accountId);
    }
}
