using GenZStyleAPP.BAL.DTOs.Collections;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Net.Http;

namespace GenZStyleApp_API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class CollectionsController : ODataController
    {
        private ICollectionRepository _collectionRepository;

        public CollectionsController(ICollectionRepository collectionRepository) 
        { 
            this._collectionRepository = collectionRepository;
        }


        #region Save Collection By PostId
        [HttpPost("odata/Posts/{postId}/SaveCollectionByPostId")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> SaveCollectionByPostId([FromRoute] int postId)
        {

            try
            {
                HttpContext httpContext = HttpContext;
                GetCollectionResponse collection   = await this._collectionRepository.SavePostToCollection(postId, httpContext);
                if (collection != null)
                {
                    return Ok(new { Message = "Save Post Into Post Successfully.", collectionResponses = collection });
                }
                else
                {
                    // Không tìm thấy tài khoản, trả về thông báo không có kết quả
                    return Ok(new { Message = "Save Post Into Post Fail" });
                }
            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
        }
        #endregion

        // GetAll
        [HttpGet("odata/Collections/Active/GetAllColletion")]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<GetCollectionResponse> collections = await this._collectionRepository.GetAllCollectionsAsync(HttpContext);
                return Ok(new
                {
                    Status = "Get List Success",
                    Data = collections
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpGet("odata/Collections/Active/Collection/{collectionId}")]
        [EnableQuery(MaxExpansionDepth = 3)]
        public async Task<IActionResult> GetCollectionByCollectionId(int collectionId)
        {
            try
            {
                GetCollectionResponse collection = await this._collectionRepository.GetCollectionByCollectionId(collectionId);

                // Kiểm tra nếu collection không tồn tại
                if (collection == null)
                {
                    return BadRequest("Collection not found. Please provide a valid collectionId.");
                }

                return Ok(new
                {
                    Status = "Get Collection By Id Success",
                    Data = collection
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
