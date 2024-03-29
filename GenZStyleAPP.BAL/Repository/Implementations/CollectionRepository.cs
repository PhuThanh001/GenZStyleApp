using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Collections;
using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.DTOs.PostLike;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class CollectionRepository : ICollectionRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CollectionRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        // Inside PostRepository class

        public async Task<GetCollectionResponse> SavePostToCollection(int postId, HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                Post post = await _unitOfWork.PostDAO.GetPostByIdAsync(postId); // chỗ này chỉ tk phú thêm bên hàm DAO zô 
                var likes = await _unitOfWork.LikeDAO.GetLikeByPostIdAysn(postId);// chỗ này chỉ tk phú thêm bên hàm DAO zô 
                var likeresponse = _mapper.Map<List<GetPostLikeCollection>>(likes);
                var comments = await _unitOfWork.CommentDAO.GetCommentByPostIdAss(postId);// chỗ này chỉ tk phú thêm bên hàm DAO zô ;
                var commentresponse = _mapper.Map<List<GetCommentResponse>>(comments);
                // Check if the post is already saved in the collection
                var existingCollection = await _unitOfWork.CollectionDAO.GetAllCollectionsByPostId(postId);

                if (existingCollection == null)
                {
                    // Create a new Collection object if the post is not already saved
                    Collection collection = new Collection
                    {
                        AccountId = accountStaff.AccountId,
                        CategoryId = 1,
                        PostId = post.PostId,
                        Name = post.Content,
                        Image_url = post.Image,
                        Type = 1,
                        IsSaved = true, // Set the IsSaved flag to true

                    };

                    // Add the Collection object using the CollectionDAO
                    await _unitOfWork.CollectionDAO.AddNewCollection(collection);
                    await _unitOfWork.CommitAsync();

                    // Retrieve hashtags for the post
                    List<string> hashtags = new List<string>();

                    if (post.HashPosts != null && post.HashPosts.Any())
                    {
                        // Ensure that each HashPost has a valid Hashtag
                        foreach (var hashPost in post.HashPosts)
                        {
                            if (hashPost.Hashtag != null && !string.IsNullOrEmpty(hashPost.Hashtag.Name))
                            {
                                hashtags.Add(hashPost.Hashtag.Name);
                            }
                        }
                    }

                    // Create a DTO object representing the collection data
                    GetCollectionResponse collectionResponse = new GetCollectionResponse
                    {
                        Id = collection.Id,
                        AccountId = collection.AccountId,
                        CategoryId = collection.CategoryId,
                        PostId = collection.PostId,
                        Content = collection.Name,
                        Image = collection.Image_url,
                        Type = collection.Type,
                        IsSaved = collection.IsSaved,
                        Hashtags = hashtags, // Assign hashtags to the response object
                        Likes = likeresponse,
                        Comments = commentresponse
                    };

                    // Return DTO object representing the collection data
                    return collectionResponse;
                }
                else
                {
                    // Update the IsSaved flag to true if the post is already saved
                    existingCollection.IsSaved = !existingCollection.IsSaved; // Toggle the IsSaved flag
                    await _unitOfWork.CommitAsync();

                    // Retrieve hashtags for the post
                    List<string> hashtags = new List<string>();

                    if (post.HashPosts != null && post.HashPosts.Any())
                    {
                        // Ensure that each HashPost has a valid Hashtag
                        foreach (var hashPost in post.HashPosts)
                        {
                            if (hashPost.Hashtag != null && !string.IsNullOrEmpty(hashPost.Hashtag.Name))
                            {
                                hashtags.Add(hashPost.Hashtag.Name);
                            }
                        }
                    }

                    // Create a DTO object representing the existing collection data
                    GetCollectionResponse collectionResponse = new GetCollectionResponse
                    {
                        Id = existingCollection.Id,
                        AccountId = existingCollection.AccountId,
                        CategoryId = existingCollection.CategoryId,
                        PostId = existingCollection.PostId,
                        Content = existingCollection.Name,
                        Image = existingCollection.Image_url,
                        Type = existingCollection.Type,
                        IsSaved = existingCollection.IsSaved,
                        Hashtags = hashtags, // Assign hashtags to the response object
                        Likes = likeresponse,
                        Comments = commentresponse
                    };

                    // Return DTO object representing the existing collection data
                    return collectionResponse;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception(ex.Message); // or return null, depending on your error handling strategy
            }
        }
        #region GetAllCollection
        public async Task<List<GetCollectionResponse>> GetAllCollectionsAsync(HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                var collections = await _unitOfWork.CollectionDAO.GetAllCollections(accountStaff.AccountId);

                List<GetCollectionResponse> responseList = new List<GetCollectionResponse>();

                foreach (var collection in collections)
                {
                    var response = _mapper.Map<GetCollectionResponse>(collection);
                    List<string> hashtags = new List<string>(); // Khởi tạo một danh sách rỗng để tránh lỗi null

                    if (collection.Post.HashPosts != null)
                    {
                        // Nếu collection.Post.HashPosts không null, thực hiện lấy thông tin Hashtag
                        hashtags = collection.Post.HashPosts.Select(h => h.Hashtag.Name).ToList();
                    }
                    response.Hashtags = hashtags;
                    // Lấy thông tin Like của bài post
                    response.Likes = await GetPostLikesAsync(collection.PostId);


                    // Lấy thông tin Comment của bài post
                    response.Comments = await GetPostCommentsAsync(collection.PostId);

                    responseList.Add(response);
                }

                return responseList;
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
        #endregion

        // Phương thức để lấy thông tin Like của bài post
        private async Task<List<GetPostLikeCollection>> GetPostLikesAsync(int postId)
        {
            var post = await _unitOfWork.PostDAO.GetPostByIdAsync(postId);
            if (post != null)
            {
                return post.Likes
                    .Where(like => like.isLike == true)
                    .Select(like => new GetPostLikeCollection
                    {
                        PostId = like.PostId,
                        LikeBy = like.LikeBy
                        // Cần thêm phần mapping thông tin từ Account sang GetAccountResponse nếu cần
                    }).ToList();
            }
            return new List<GetPostLikeCollection>();
        }
        // Phương thức để lấy thông tin Comment của bài post
        private async Task<List<GetCommentResponse>> GetPostCommentsAsync(int postId)
        {
            var post = await _unitOfWork.PostDAO.GetPostByIdAsync(postId);
            if (post != null)
            {
                return post.Comments
                    .Select(comment => new GetCommentResponse
                    {
                        CommentId = comment.CommentId,
                        CreateAt = comment.CreateAt,
                        Content = comment.Content,
                        CommentBy = comment.CommentBy
                        // Cần thêm phần mapping thông tin từ Account sang GetAccountResponse nếu cần
                    }).ToList();
            }
            return new List<GetCommentResponse>();
        }

        #region GetCollectionByCollectionId
        public async Task<GetCollectionResponse> GetCollectionByCollectionId(int collectionId)
        {
            try
            {
                Collection collections = await _unitOfWork.CollectionDAO.GetAllCollectionsByCollectionId(collectionId);
                if (collections == null)
                {
                    throw new NotFoundException("CollectionId does not exist in the system.");
                }
                return _mapper.Map<GetCollectionResponse>(collections);
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
        #endregion


    }
}
