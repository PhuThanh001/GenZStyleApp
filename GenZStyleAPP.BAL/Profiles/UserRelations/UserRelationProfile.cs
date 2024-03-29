using GenZStyleAPP.BAL.DTOs.PostLike;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.UserRelations;
using AutoMapper;

namespace GenZStyleAPP.BAL.Profiles.UserRelations
{
    public class UserRelationProfile : Profile
    {
        public UserRelationProfile()
        {
            CreateMap<UserRelation, GetUserRelationResponse>().ReverseMap();
        }
    }
}
