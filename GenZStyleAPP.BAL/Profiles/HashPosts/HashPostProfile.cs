using AutoMapper;
using GenZStyleAPP.BAL.DTOs.HashTags;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.HashPosts;

namespace GenZStyleAPP.BAL.Profiles.HashPosts
{
    public class HashPostProfile: Profile
    {
        public HashPostProfile() 
        {
            CreateMap<HashPost, GetHashPostsResponse>().ReverseMap();
            CreateMap<HashPost, GetHashPostByHashtag>().ReverseMap();
        }
    }
}
