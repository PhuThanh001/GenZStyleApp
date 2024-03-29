using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.Notifications;

namespace GenZStyleAPP.BAL.Profiles.Notifications
{
    public class NotificationProfile: Profile
    {
        public NotificationProfile() 
        {
            CreateMap<Notification, GetNotificationResponse>().ReverseMap();

        }
    }
}
