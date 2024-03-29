using GenZStyleAPP.BAL.DTOs.Package;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace GenZStyleAPP.BAL.Profiles.Packages
{
    public class PackageProfiless : Profile
    {
        public PackageProfiless()
        {

            CreateMap<Package, GetPackageResponse>().ReverseMap();
        }
    }
}
