using FluentValidation;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Reports;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    public class ReportsController : ODataController
    {
        private IReportRepository _reportRepository;
        private IOptions<FireBaseImage> _firebaseImageOptions;
        private IValidator<AddReportRequest> _reportValidator;
        private IValidator<AddReporterRequest> _reporterValidator;
        

        public ReportsController(IReportRepository reportRepository, IOptions<FireBaseImage> firebaseImageOptions, IValidator<AddReportRequest> reportValidator, IValidator<AddReporterRequest> reporterValidator, ILogger<ReportsController> logger)
        {
            _reportRepository = reportRepository;
            _firebaseImageOptions = firebaseImageOptions;
            _reportValidator = reportValidator;
            _reporterValidator = reporterValidator;
            
        }


        [HttpGet("odata/GetAllReportByPost/Reports")]
        [EnableQuery]
        public async Task<IActionResult> GetAllReportByPost()
        {
            try
            {
                List<GetReportResponse> postReports =  this._reportRepository.GetPostReports();
                return Ok(postReports);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Microsoft.AspNetCore.Mvc.HttpGet("odata/GetAllReportByUser/Reports")]
        /*[HttpGet("odata/Reports/Active/GetAllReportByUser")]*/
        [EnableQuery]
        public async Task<IActionResult> GetAllReportByUser()
        {
            try
            {
                List<GetReportResponse> userReports = await this._reportRepository.GetUserReports();
                return Ok(userReports);
            }
            catch (Exception ex)

            {
                return StatusCode(500, ex.Message);
            }


        }

        [HttpGet("odata/Reports/Active/GetPostReportsByReportId/{reportID}")]
        //[EnableQuery(MaxExpansionDepth = 3)]
        public async Task<IActionResult> GetPostReportsByReportId(int reportID)
        {
            try
            {
                List<GetReportResponse> report = await this._reportRepository.GetPostReportsByReportId(reportID);

                // Kiểm tra nếu user không tồn tại
                if (report == null)
                {
                    return BadRequest("User not found. Please provide a valid userId.");
                }

                return Ok(new
                {
                    Message = "Get Report By ReportID Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("odata/Reports/Active/GetUserReportsByReportId/{reportId}")]
        //[EnableQuery(MaxExpansionDepth = 3)]
        public async Task<IActionResult> GetUserReportsByReportId(int reportId)
        {
            try
            {
                List<GetReportResponse> report = await this._reportRepository.GetUserReportsByReportId(reportId);

                // Kiểm tra nếu user không tồn tại
                if (report == null)
                {
                    return BadRequest("User not found. Please provide a valid userId.");
                }

                return Ok(new
                {
                    Message = "Get Report By ReportID Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


        [HttpGet("odata/Reports/Active/Report/{reportname}")]
        //[EnableQuery(MaxExpansionDepth = 3)]
        public async Task<IActionResult> ActiveReportByReportName(string reportname)
        {
            try
            {
                GetReportResponse report = await this._reportRepository.GetActiveReportName(reportname);

                // Kiểm tra nếu user không tồn tại
                if (report == null)
                {
                    return BadRequest("User not found. Please provide a valid userId.");
                }

                return Ok(new
                {
                    Status = "Get User By Id Success",
                    Data = report
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        //[HttpGet("odata/Reports/Active/ProcessReportStatusAsync/{reportId}")]
        ////[EnableQuery(MaxExpansionDepth = 3)]
        //public async Task<IActionResult> ProcessReportStatusAsync(int reportId, ReportAction reportAction)
        //{
        //    try
        //    {
        //        // Gọi phương thức xử lý trạng thái báo cáo từ repository hoặc service
        //        List<GetReportResponse> result = await _reportRepository.ProcessReportStatusAsync(reportId, reportAction);

        //        // Kiểm tra nếu kết quả không rỗng thì trả về danh sách báo cáo
        //        if (result != null && result.Count > 0)
        //        {
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            // Trả về thông báo nếu không có báo cáo nào được xử lý
        //            return NotFound("No reports found for the given reportId.");
        //        }
        //    }
        //    catch (NotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }


        //}


        #region Create New Report
        [HttpPost("odata/Report/AddNewReport")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Post([FromForm] AddReportRequest addReportRequest)
        {
            try
            {
                var resultValid = await _reportValidator.ValidateAsync(addReportRequest);
                if (!resultValid.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(resultValid);
                    throw new BadRequestException(error);
                }
                GetReportResponse report = await this._reportRepository.CreateNewReportByPostIdAsync(addReportRequest, HttpContext);
                return Ok(new
                {
                    Status = "Add Report Success",
                    Data = Created(report)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Create New Report
        [HttpPost("odata/Report/AddNewReportByReporter")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> AddReportByReporter([FromForm] AddReporterRequest addReportsRequest)
        {
            try
            {
                var resultValid = await _reporterValidator.ValidateAsync(addReportsRequest);
                if (!resultValid.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(resultValid);
                    throw new BadRequestException(error);
                }
                GetReportResponse report = await this._reportRepository.CreateNewReportByReporterIdAsync(addReportsRequest, HttpContext);
                return Ok(new
                {
                    Status = "Add Report Success",
                    Data = Created(report)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        //ban Report
        [EnableQuery]
        [HttpPut("odata/Reports/{id}/{status}/BanReportByPostId")]
        //[PermissionAuthorize("Store Owner")]
        public async Task<IActionResult> BanReportByPostId([FromRoute] int id, [FromRoute] string status)
        {
            try
            {
                List<GetReportResponse> reports = await this._reportRepository.BanReportAsync(id, status);

                return Ok(new
                {
                    Message = "Accept Successfully",
                    Data = reports
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    Status = -1,
                    Message = "Ban Report Fail: " + ex.Message
                });
            }
        }
        //ban User
        [EnableQuery]
        [HttpPut("odata/Reports/{reportId}/{status}/BanUserByReporterId")]
        //[PermissionAuthorize("Store Owner")]
        public async Task<IActionResult> BanUserByReporterId([FromRoute] int reportId, [FromRoute] string status)
        {
            try
            {
                List<GetReportResponse> reports = await this._reportRepository.BanUserAsync(reportId, status);

                return Ok(new
                {
                    Message = "Accept Successfully",
                    Data = reports
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    Status = -1,
                    Message = "Ban Report Fail: " + ex.Message
                });
            }
        }


        //#region Delete Report

        //[HttpDelete("Report/{postId}")]
        //[EnableQuery]
        ////[PermissionAuthorize("Staff")]
        //public async Task<IActionResult> Delete([FromRoute] int postId)
        //{
        //    await this._reportRepository.DeleteReportAsync(postId, this.HttpContext);
        //    return Ok(new
        //    {
        //        Status = "Delete Report Success",

        //    });
        //}
        //#endregion

    }

}

