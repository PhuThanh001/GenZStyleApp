using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.PostLike;
using GenZStyleAPP.BAL.DTOs.Reports;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class LikeRepository : ILikeRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public LikeRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetPostLikeResponse>> GetAllAccountByLikes(int postId)
        {
            try
            {
                List<Like> likes = await this._unitOfWork.LikeDAO.GetAllAccountByLikes(postId);
                return this._mapper.Map<List<GetPostLikeResponse>>(likes);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        public async Task<GetPostLikeResponse> GetLikeByPostIdAsync(int postId, HttpContext httpContext)
        {
            try
            {
                var post = await _unitOfWork.PostDAO.GetPostByIdAsync(postId);
                if (post == null)
                {
                    throw new NotFoundException("PostId does not exist in system.");
                }
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                var like = await _unitOfWork.LikeDAO.GetLikeByPostIdAndAccount(postId, account.AccountId);
                post.TotalLike += 1;
                Like likes = new Like
                {
                    LikeBy = account.AccountId,
                    PostId = postId,
                    /*isLike = (like.isLike == true) ? false : (like.isLike != null ? like.isLike : true),*/
                    isLike = like != null ? !like.isLike : true,
                    Post = post,
                    Account = account,

                };

                if (like != null)
                {
                    likes.isLike = !like.isLike;                   
                }
                
                if (likes.isLike == true) 
                {
                    Notification notification = new Notification
                    {
                        CreateAt = DateTime.Now,
                        AccountId = post.AccountId,
                        Message = account.Username + " " + "đã like bài viết của bạn",
                        Account = account,
                    };

                    await _unitOfWork.NotificationDAO.AddNotiAsync(notification);
                }


                await _unitOfWork.LikeDAO.AddLikeAsync(likes);
                _unitOfWork.LikeDAO.ChangeLike(post);

                await _unitOfWork.CommitAsync();
                return _mapper.Map<GetPostLikeResponse>(likes);

            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("PostId", ex.Message);
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
