using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.Reports
{
    public class ReportProfile: Profile
    {
        public ReportProfile() 
        { 
            CreateMap<Report, GetReportResponse>().ReverseMap();
           
        }
    }
}
