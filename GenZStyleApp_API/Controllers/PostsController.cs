using FluentValidation;
using FluentValidation.Results;
using GenZStyleApp.DAL.DAO;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using System.ComponentModel.DataAnnotations;

namespace GenZStyleApp_API.Controllers
{
    
    
    public class PostsController : ODataController
    {
        private IPostRepository _postRepository;
        private IValidator<AddPostRequest> _postValidator;
        private IOptions<FireBaseImage> _firebaseImageOptions;
        private IValidator<UpdatePostRequest> _updatePostValidator;
        public PostsController(IPostRepository productRepository,
            IValidator<AddPostRequest> postProductValidator,
            IOptions<FireBaseImage> firebaseImageOptions,
            IValidator<UpdatePostRequest> updatePostValidator)


        {
            this._updatePostValidator = updatePostValidator;
            this._postRepository = productRepository;
            this._postValidator = postProductValidator;
            this._firebaseImageOptions = firebaseImageOptions;
        }

        #region Get Posts
        [HttpGet("odata/GetPosts")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<GetPostResponse> post = await this._postRepository.GetPostsAsync(HttpContext);
                return Ok(post);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        #endregion
        [HttpGet("odata/Posts/Active/CountAllPost")]
        [EnableQuery]
        public async Task<IActionResult> CountAllPost()
        {
            try
            {
                List<GetPostResponse> posts = await this._postRepository.GetActivePosts();
                int totalPost = posts.Count;
                return Ok(new
                {
                    TotalPost = totalPost,
                });

            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }

        }

        [HttpGet("odata/Posts/Active/Post")]
        [EnableQuery]
        public async Task<IActionResult> ActivePosts()
        {
            List<GetPostResponse> products = await this._postRepository.GetActivePosts();
            return Ok(products);
        }
        [HttpGet("odata/Posts/Active/GetActivePosts")]
        [EnableQuery]
        public IActionResult GetActivePosts()
        {
            List<GetPostResponse> products =  this._postRepository.GetActivePostss();
            return Ok(products);
        }

        #region Get Post Detail By Id
        [HttpGet("odata/Posts/{key}/GetPostById")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Get([FromRoute] int key)
        {
            try
            {
                GetPostResponse post = await this._postRepository.GetPostDetailByIdAsync(key);
                if(post != null)
                {
                    return Ok(new { Message = "Get By ID Successfully.", posts = post});
                }
                else
                {
                    // Không tìm thấy tài khoản, trả về thông báo không có kết quả
                    return Ok(new { Message = "Not found ID in system." });
                }
            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
            
        }
        #endregion

        #region Get Post By AccountId
        [HttpGet("odata/Posts/{accountId}/GetPostByAccountId")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> GetByAccountId([FromRoute] int accountId)
        {
            
            try
            {
                List<GetPostResponse> post = await this._postRepository.GetPostByAccountIdAsync(accountId);
                if (post != null)
                {
                    return Ok(new { Message = "Get By ID Successfully.", posts = post });
                }
                else
                {
                    // Không tìm thấy tài khoản, trả về thông báo không có kết quả
                    return Ok(new { Message = "Not found AccountID in system." });
                }
            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
        }
        #endregion

        #region View Profile By Gender
        [HttpGet("odata/Posts/{gender:bool}/GetPostByGender")]
        [EnableQuery]
        //[PermissionAuthorize("Customer", "Store Owner")]
        public async Task<IActionResult> GetByGender([FromRoute] bool gender)
        {
            try
            {
                List<GetPostResponse> post = await this._postRepository.GetPostByGenderAsync(gender);               
                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion
    

    #region Create Post
    [HttpPost("odata/Post/AddNewPost")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Post([FromForm] AddPostRequest addPostRequest)
        {
            try
            {
                var resultValid = await _postValidator.ValidateAsync(addPostRequest);
                if (!resultValid.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(resultValid);
                    throw new BadRequestException(error);
                }
                GetPostResponse post = await this._postRepository.CreateNewPostAsync(addPostRequest, _firebaseImageOptions.Value, HttpContext);
                return Ok(new
                {
                    Status = "Add Post Success",
                    Data = Created(post),
                    
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        #endregion

        #region Update Post
        [HttpPut("Post/{key}/UpdatePost")]
        [EnableQuery]
        //[PermissionAuthorize("Customer", "Store Owner")]
        public async Task<IActionResult> Put([FromRoute] int key, [FromForm] UpdatePostRequest updatePostRequest)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = await _updatePostValidator.ValidateAsync(updatePostRequest);
                if (!validationResult.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(validationResult);
                    throw new BadRequestException(error);
                }
                GetPostResponse post = await this._postRepository.UpdatePostProfileByPostIdAsync(key,_firebaseImageOptions.Value,
                                                                                                                    updatePostRequest,HttpContext);

                if (post != null)
                {
                    return Ok(new { Message = "Update Post Successfully.", posts = Updated(post) });
                }
                else
                {
                    
                    return Ok(new { Message = "Update Post Fail." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("odata/GetPost/User/Follow")]
        [EnableQuery]
        public async Task<IActionResult> GetPostForUser()
        {
            List<GetPostResponse> products = await this._postRepository.GetPostByUserFollowId(HttpContext);
            return Ok(products);
        }
        #endregion

        [EnableQuery]
        [HttpPut("odata/Posts/{key}/BanPostByPostId")]
        //[PermissionAuthorize("Store Owner")]
        public async Task<IActionResult> BanPost([FromRoute] int key)
        {
            try
            {
                GetPostResponse post = await this._postRepository.BanPostAsync(key, HttpContext);

                if (post != null)
                {
                    return Ok(new
                    {
                        Status = " Ban Post Successfully",
                        Data = post
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Status = -1,
                        Message = "Ban Post Fail"
                    });

                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
