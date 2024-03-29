using GenZStyleAPP.BAL.DTOs.HashTags;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.ChatHistorys;
using AutoMapper;

namespace GenZStyleAPP.BAL.Profiles.Messages
{
    public class MessageProfile : Profile
    {
        public MessageProfile() 
        {
            CreateMap<Message, GetChatHistoryResponse>().ReverseMap();
            CreateMap<Message, MessageViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.From, opt => opt.MapFrom(x => x.AccountSender.Username))
                .ForMember(dst => dst.To, opt => opt.MapFrom(x => x.Inbox.Name))
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.AccountSender.User.AvatarUrl));               
                
                

        }
    }
}
