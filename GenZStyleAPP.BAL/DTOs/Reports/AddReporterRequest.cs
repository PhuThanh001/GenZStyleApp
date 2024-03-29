using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Reports
{
    public class AddReporterRequest
    {
        public int ReporterId { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
    }
}
