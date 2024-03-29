using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.DTOs.Posts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.Posts
{
    public class PostProfile: Profile
    {
        public PostProfile() 
        {
            CreateMap<AddPostRequest, Post>().ReverseMap();
            //CreateMap<Post, GetPostResponse>().ForMember(dest => dest.FashionItems, opt => opt.MapFrom(src => src.FashionItems)).ReverseMap();
            CreateMap<Post, UpdatePostRequest>().ReverseMap();
            CreateMap<Post, GetPostResponse>().ReverseMap();
            CreateMap<Post, GetPostForSearch>().ReverseMap();
            CreateMap<Post, GetCollectionPostResponse>().ReverseMap();
            /*CreateMap<Post, GetCommentResponse>().ReverseMap();*/

            CreateMap<Post, GetCommentResponse>()
    .ForMember(dest => dest.CommentBy, opt => opt.MapFrom(src => src.AccountId))
    .ReverseMap();



        }
        
    }
}
