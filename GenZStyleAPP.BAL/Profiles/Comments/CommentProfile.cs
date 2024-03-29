using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.Comments
{
    public class CommentProfile : Profile
    {   

        public CommentProfile() 
        {
            CreateMap<Comment, GetCommentResponse>().ReverseMap(); 
            
        }
        
    }
}
