using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Invoices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Package
{
    public class GetPackageResponse
    {
        [Key]
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal Cost { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int IsStatus { get; set; }
    }
}
