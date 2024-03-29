using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Authencications
{
    public class GetLoginEmailRequest
    {
        [Key]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
