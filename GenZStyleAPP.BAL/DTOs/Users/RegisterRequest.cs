using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Users
{
    public class RegisterRequest
    {
        [Key]
        public string Email { get; set; }

        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }

        public decimal? Height { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string Phone { get; set; }
        public IFormFile? Avatar { get; set; }
        public bool? Gender { get; set; }
        public DateTime Dob { get; set; }
    }
}
