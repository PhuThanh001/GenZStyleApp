using GenZStyleAPP.BAL.DTOs.Reports;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenZStyleAPP.BAL.Repository.Implementations.ReportRepository;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IReportRepository
    {
        public List<GetReportResponse> GetPostReports();
        public Task<List<GetReportResponse>> GetUserReports();
        public Task<GetReportResponse> GetActiveReportName(string reportname);

        public Task<List<GetReportResponse>> GetPostReportsByReportId(int reportId);
        public Task<List<GetReportResponse>> GetUserReportsByReportId(int reportId);
        public Task<List<GetReportResponse>> BanReportAsync(int reportId, string status);
        //public Task DeleteReportAsync(int postId, HttpContext httpContext);
        //public Task<List<GetReportResponse>> ProcessReportStatusAsync(int reportId, ReportAction reportAction);
        public Task<GetReportResponse> CreateNewReportByPostIdAsync(AddReportRequest addReportRequest, HttpContext httpContext);
        public Task<GetReportResponse> CreateNewReportByReporterIdAsync(AddReporterRequest addReportRequest, HttpContext httpContext);
        public Task<List<GetReportResponse>> BanUserAsync(int reportId, string status);
    }
}
