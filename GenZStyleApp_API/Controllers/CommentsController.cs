using FluentValidation;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.Repository.Implementations;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    public class CommentsController : ODataController
    {
        private ICommentRepository _commentRepository;
        private IValidator<GetCommentRequest> _getCommentRequestvalidator;

        public CommentsController(ICommentRepository commentRepository,IValidator<GetCommentRequest> validator )
        {
            _commentRepository = commentRepository;
            _getCommentRequestvalidator = validator;
        }
        [HttpGet("odata/Comment/{PostId}")]
        public async Task<IActionResult> GetAllComment(int PostId)
        {
            List<GetCommentResponse> result = await _commentRepository.GetCommentByPostId(PostId);
            return Ok(result);
        }
        
        [EnableQuery]
        [HttpPost("odata/Comment/{key}")]

        public async Task<IActionResult> Post([FromRoute] int key, [FromBody]  GetCommentRequest commentRequest)
        {
            var resultValid = _getCommentRequestvalidator.Validate(commentRequest);
            if (!resultValid.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(resultValid);
                throw new BadRequestException(error);
            }
            /*GetCommentResponse getCommentResponse =*/ await this._commentRepository.UpdateCommentByPostId(commentRequest, key, HttpContext);
            return Ok();
        }

        [EnableQuery]
        [HttpDelete("odata/Comment/DeleteComment/{key}")]

        public async Task<IActionResult> DeleteComment([FromRoute] int key)
        {
            
            await this._commentRepository.DeleteCommentById(key);
            return Ok(new
            {
                Status = "Delete Comment Success"
            });
        }




    }
}
