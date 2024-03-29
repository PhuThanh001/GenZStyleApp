using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; } //người đi report
        public int? AccuseeId { get; set; } // người bị report
                
        public int? PostId { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        
        public int IsStatusReport { get; set; }
        [ForeignKey("ReporterId")]
        public Account Account { get; set; }
        
        public Post Post { get; set; }

    }
}
