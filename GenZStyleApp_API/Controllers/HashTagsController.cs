using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.HashTags;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.Repository.Implementations;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using System.ComponentModel.DataAnnotations;
using GenZStyleAPP.BAL.DTOs.Posts;

namespace GenZStyleApp_API.Controllers
{
    
    public class HashTagsController : ODataController
    {
        private IHashTagRepository _hashTagRepository;
        private IValidator<GetHashTagRequest> _getHashTagValidator;
        private IOptions<FireBaseImage> _firebaseImageOptions;

        public HashTagsController(IHashTagRepository hashTagRepository,
                                  IValidator<GetHashTagRequest> getHashTagRequest,
                                    IOptions<FireBaseImage> firebaseImageOptions)
        {
            this._hashTagRepository = hashTagRepository;
            this._getHashTagValidator = getHashTagRequest;
            this._firebaseImageOptions = firebaseImageOptions;
        }

        #region SearchByHashTagName
        [HttpGet("odata/HashTags/SearchByHashTagName")]

        public async Task<IActionResult> SearchByHashTagName([FromQuery] string hashtag)
        {
            try
            {

                List<GetPostForSearch> hashtagDTOs = await _hashTagRepository.SearchByHashTagName(hashtag);
                List<GetHashTagResponse> hashtagDTOss = new List<GetHashTagResponse>();
                // Nếu muốn thực hiện bất kỳ xử lý hoặc kiểm tra nào đó trước khi trả kết quả, bạn có thể thêm vào đây

                if (hashtagDTOs.Count > 0)
                {
                    // Thành công, trả về thông báo thành công và danh sách tài khoản
                    return Ok(new { Message = "Find HashTagName Successfully.", HashTags = hashtagDTOs });
                }
                else
                {
                    // Không tìm thấy tài khoản, trả về thông báo không có kết quả
                    return Ok(new { Message = "Not Found HashTagName in the system.", HashTags = hashtagDTOss });
                }
            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
        }
        #endregion
        #region GetHashTags
        [EnableQuery]
        [HttpGet("odata/Hashtags/GetHashTag")]
        public async Task<IActionResult> Get()
        {


            List<GetAllHashTag> result = await _hashTagRepository.GetHashTagsAsync();
            return Ok(result);
        }
        #endregion


        [EnableQuery]
        [HttpPost("odata/Hashtags")]
        public async Task<IActionResult> Post([FromForm] GetHashTagRequest hashTagRequest)
        {
            var validationResult = await _getHashTagValidator.ValidateAsync(hashTagRequest);
            if (!validationResult.IsValid)   
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }
            GetHashTagResponse hashTagResponse = await _hashTagRepository.AddNewHashTag(_firebaseImageOptions.Value, hashTagRequest);
            return Ok(hashTagResponse);
        }


    }


}

