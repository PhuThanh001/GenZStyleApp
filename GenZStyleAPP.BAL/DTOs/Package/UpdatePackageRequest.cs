using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Package
{
    public class UpdatePackageRequest
    {
        public string PackageName { get; set; }
        public decimal Cost { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
    }
}
