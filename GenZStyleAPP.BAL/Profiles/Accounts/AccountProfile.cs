using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.Users;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.Accounts
{
    public class AccountProfile : Profile
    {
        
        public AccountProfile() 
        {
            CreateMap<Account, GetAccountResponse>().ReverseMap();

            
        }
    }
    }

