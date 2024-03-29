using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Blogger
    {
        [Key]
        public int AccountID { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string AvatarURL { get; set; }
        public bool Gender { get; set; }
        public DateTime Dob { get; set; }
        public virtual Account Account { get; set; }
    }
}
