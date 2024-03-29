using GenZStyleApp.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.HashTags
{
    public class GetHashTagRequest
    {

        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
