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
    public class PackageProfile : Profile
    {
        public PackageProfile()
        {

            CreateMap<Package, GetPackageResponse>().ReverseMap();
            CreateMap<Package, GetPackageRequest>().ReverseMap();
            CreateMap<Package, UpdatePackageRequest>().ReverseMap();
        }
    }
}
