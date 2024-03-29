using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.PostLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.PostLike
{
    public class PostLikeProfile : Profile
    {
        

        public PostLikeProfile()
        {
            CreateMap<Post ,GetPostLikeResponse>()
                /*.ForMember(dest => dest.LikeBy, opt => opt.MapFrom(src => src.AccountId))*/
                .ReverseMap();
            
            CreateMap<Like, GetPostLikeResponse>().ReverseMap();
            CreateMap<Like, GetPostLikeCollection>().ReverseMap();

            
        }
    }
}
