using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CommentRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }


        public async Task <List<GetCommentResponse>> GetCommentByPostId(int id)
        {
            try
            {   
                
                var comments = await _unitOfWork.CommentDAO.GetCommentByPostIdAsync(id);
                List<GetCommentResponse> responses = new List<GetCommentResponse>();
                foreach (var comment in comments) 
                {
                    Account account = await _unitOfWork.AccountDAO.GetAccountById(comment.CommentBy);
                    GetCommentResponse response = new GetCommentResponse
                    {
                        CommentId = comment.CommentId,
                        CreateAt = comment.CreateAt,
                        Content = comment.Content,
                        CommentBy = comment.CommentBy,
                        Username = account.Username,
                        image = account.User.AvatarUrl,

                    };
                    responses.Add(response);
                }
                
                /*return _mapper.Map<List<GetCommentResponse>>(post);*/
                return responses;
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        public async Task DeleteCommentById(int id)
        {
            try
            {
                var comment = await _unitOfWork.CommentDAO.GetCommentByIdAsync(id);
                if(comment == null)
                {
                    throw new NotFoundException("Comment does not exist in system.");
                }
                await _unitOfWork.CommentDAO.DeleteCommentById(comment);
                await _unitOfWork.CommitAsync();
                
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }


        public async Task UpdateCommentByPostId(GetCommentRequest commentRequest ,int PostId,HttpContext httpContext )
        {
            try
            {
                var post = await _unitOfWork.PostDAO.GetPostByIdAsync(PostId);
                if (post == null)
                {
                    throw new NotFoundException("PostId does not exist in system.");
                }

                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                post.TotalComment += 1;
                Comment comment = new Comment
                {
                    Content = commentRequest.Content,
                    CreateAt = commentRequest.CreateAt,
                    PostId = PostId,
                    CommentBy = account.AccountId,
                    Account = account,
                    Post = post,
                };
                Notification notification = new Notification
                {
                    CreateAt = DateTime.Now,
                    AccountId = post.AccountId,
                    Message = account.Lastname + " " + "đã bình luận bài viết của bạn",
                    Account = account,
                };

                /*post.Comments.Add(comment);*/
                await _unitOfWork.CommentDAO.AddCommentAsync(comment);
                     this._unitOfWork.PostDAO.UpdatePostComment(post);
                await _unitOfWork.NotificationDAO.AddNotiAsync(notification);

                await _unitOfWork.CommitAsync();
                /*return _mapper.Map<GetCommentResponse>(post);*/



            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        } 




    }
}

