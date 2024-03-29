using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Posts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Reports
{
    public class GetReportResponse
    {
        public int Id { get; set; }
        public int? AccuseeId { get; set; }
        public int? PostId { get; set; }
        public int ReporterId { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public int IsStatusReport { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        
        
        /*[ForeignKey("PostId")]*/
        /*public GetPostResponse Post { get; set; }*/
    }
}
