using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Package;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IPackageRepository
    {
        public Task<GetPackageResponse> PurcharePackage(int PackageId, HttpContext httpContext);
        public Task<GetPackageResponse> CreateNewPackageAsync(GetPackageRequest addPackageRequest, FireBaseImage fireBaseImage, HttpContext httpContext);
        public Task<GetPackageResponse> UpdatePackageByIdAsync(int packageId,
                                                                                     FireBaseImage fireBaseImage,
                                                                                     UpdatePackageRequest updatePackageRequest, HttpContext httpContext);
        public Task<List<GetPackageResponse>> GetPackagesAsync();
    }
}
