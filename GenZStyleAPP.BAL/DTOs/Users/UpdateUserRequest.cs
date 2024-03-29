using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Users
{
    public class UpdateUserRequest
    {
        public string? City { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }

        public decimal? Height { get; set; }
        public bool? Gender { get; set; }
        public DateTime Dob { get; set; }
    }
}
