using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleApp.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.Rooms
{
    public class InboxProfile : Profile
    {
        public InboxProfile() {
            CreateMap<Inbox, RoomViewModel>().ReverseMap(); 
        
        }
        
    }
}
