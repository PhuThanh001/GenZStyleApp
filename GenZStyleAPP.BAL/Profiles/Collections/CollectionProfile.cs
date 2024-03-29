using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.Collections
{
    public class CollectionProfile: Profile
    {
        public CollectionProfile() 
        {
            CreateMap<Collection, GetCollectionResponse>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image_url))
                .ReverseMap();

        }
    }
}
