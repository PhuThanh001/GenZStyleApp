using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.HashTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.HashTags
{
    public class HashTagProfile : Profile
    {
        public HashTagProfile() 
        { 
            CreateMap<Hashtag, GetHashTagResponse>().ReverseMap();
            CreateMap<Hashtag, GetAllHashTag>().ReverseMap();
        
        }
    }
}
