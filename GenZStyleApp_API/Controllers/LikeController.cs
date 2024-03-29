using BMOS.BAL.Authorization;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.PostLike;
using GenZStyleAPP.BAL.DTOs.Reports;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    
    public class LikeController : ODataController
    {
        private ILikeRepository _likeRepository;

        public LikeController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        #region Like
        [HttpGet("odata/PostLikes/GetPostId/{key}")]
        [EnableQuery]
        //[PermissionAuthorize("User")]
        public async Task<IActionResult> Get([FromRoute] int key)
        {
            GetPostLikeResponse like = await this._likeRepository.GetLikeByPostIdAsync(key, HttpContext);
            return Ok(like);
        }


        #endregion

        #region Get AllAccountByLikes
        [HttpGet("odata/Likes/GetAllAccountByLike/{postId}")]
        [EnableQuery]
        public async Task<IActionResult> GetAccountByLikes([FromRoute] int postId)
        {
            try
            {
                List<GetPostLikeResponse> likes = await _likeRepository.GetAllAccountByLikes(postId);

                if (likes != null)
                {
                    
                    return Ok(new { Message = "Get By ID Successfully.", Likes = likes });
                }
                else
                {
                    return Ok(new { Message = "No likes found for the post." });
                }
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }
        #endregion
    }
}
