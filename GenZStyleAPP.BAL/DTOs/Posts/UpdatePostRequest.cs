using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Posts
{
    public class UpdatePostRequest
    {
       
        public string Content { get; set; }
        public IFormFile? Image { get; set; }

        public List<string>? Hashtags { get; set; }
    }
}
