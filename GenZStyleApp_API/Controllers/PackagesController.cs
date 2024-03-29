using BMOS.BAL.Authorization;
using FluentValidation;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Package;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    public class PackagesController : ODataController
    {
        public IPackageRepository _packageRepository;
        private IValidator<GetPackageRequest> _packageValidator;
        private IValidator<UpdatePackageRequest> _validator1;
        private IOptions<FireBaseImage> _firebaseImageOptions;

        public PackagesController(IPackageRepository packageRepository, IValidator<GetPackageRequest> validator,
            IOptions<FireBaseImage> options, IValidator<UpdatePackageRequest> validator1)
        {
            _packageRepository = packageRepository;
            _packageValidator = validator;
            _firebaseImageOptions = options;
            _validator1 = validator1;
        }

        [HttpPost("odata/Puchare/PurcharePackage")]
        [EnableQuery]
        
        public async Task<IActionResult> PurcharePackage(int PackageId)
        {
           
               try {
                    GetPackageResponse packageResponse = await this._packageRepository.PurcharePackage(PackageId, HttpContext);
                
                return Ok(new
                {
                    Status = "Purchare Package Success",
                    Data = Created(packageResponse)
                }); 
                }catch (Exception ex) 
                
                {
                    throw new Exception(ex.Message); 
                }
                
            }
        #region Create Package
        /*[PermissionAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // chỉnh sửa chỗ này để ủy quyền
        [PermissionAuthorize("ADMIN")]*/
        [HttpPost("odata/Post/AddNewPackage")]
        [EnableQuery]
        public async Task<IActionResult> Post([FromForm] GetPackageRequest addPackageRequest)
        {
            try
            {
                var resultValid = await _packageValidator.ValidateAsync(addPackageRequest);
                if (!resultValid.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(resultValid);
                    throw new BadRequestException(error);
                }
                GetPackageResponse package = await this._packageRepository.CreateNewPackageAsync(addPackageRequest, _firebaseImageOptions.Value, HttpContext);
                return Ok(new
                {
                    Status = "Add Packages Success",
                    Data = Created(package),

                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion
        #region Update Package
        [HttpPut("Package/{key}/UpdatePackage")]
        [EnableQuery]
        //[PermissionAuthorize("Customer", "Store Owner")]
        public async Task<IActionResult> Put([FromRoute] int key, [FromForm] UpdatePackageRequest updatePackageRequest)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = await _validator1.ValidateAsync(updatePackageRequest);
                if (!validationResult.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(validationResult);
                    throw new BadRequestException(error);
                }
                GetPackageResponse package = await this._packageRepository.UpdatePackageByIdAsync(key,
                                                                                                       _firebaseImageOptions.Value,
                                                                                                       updatePackageRequest, HttpContext);

                if (package != null)
                {
                    return Ok(new { Message = "Update Package Successfully.", posts = Updated(package) });
                }
                else
                {

                    return Ok(new { Message = "Update Package Fail." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion
        #region GetAllPackages
        [EnableQuery]
        [HttpGet("odata/Package/GetAllPackages")]
        public async Task<IActionResult> Get()
        {


            List<GetPackageResponse> result = await _packageRepository.GetPackagesAsync();
            return Ok(result);
        }
        #endregion

    }



}

