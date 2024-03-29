using FluentValidation;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Notifications;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.Repository.Interfaces;
using GenZStyleAPP.BAL.Validators.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;

namespace GenZStyleApp_API.Controllers
{
    
    public class NotificationController : ODataController
    {
        private INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
            //IValidator<AddPostRequest> postProductValidator,
            //IOptions<FireBaseImage> firebaseImageOptions,
            //IValidator<UpdatePostRequest> updatePostValidator)


        {
            
            this._notificationRepository = notificationRepository;
            //this._postValidator = postProductValidator;
            //this._firebaseImageOptions = firebaseImageOptions;
            //this._updatePostValidator = updatePostValidator;
        }

        #region Get Notifications
        [HttpGet("odata/Notifications/GetAllNotification")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<GetNotificationResponse> notifications = await this._notificationRepository.GetNotificationsAsync(HttpContext);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        #endregion

        
    }
}
