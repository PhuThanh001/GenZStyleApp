using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.Repository.Interfaces;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.Notifications;
using GenZStyleAPP.BAL.Helpers;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public NotificationRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        #region GetNotifications
        public async Task<List<GetNotificationResponse>> GetNotificationsAsync(HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                if (accountStaff != null)
                {
                    // Lấy danh sách thông báo của người dùng
                    List<Notification> notifications = await _unitOfWork.NotificationDAO.GetNotifications(accountStaff.AccountId);
                    return _mapper.Map<List<GetNotificationResponse>>(notifications);
                }
                else
                {
                    // Người dùng không tồn tại hoặc không có quyền truy cập vào thông báo
                    throw new Exception("Unauthorized access to notifications.");
                }
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
