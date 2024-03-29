using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class User 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int UserId { get; set; }

        public int RoleId { get; set; }
        
        public string? City { get; set; }

        public string? AvatarUrl { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }
        public decimal? Height { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Dob { get; set; }

        public Role? Role { get; set; }
        public Account? Account { get; set; }




    }
}
