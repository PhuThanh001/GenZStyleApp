using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Authencications
{
    public class GoogleLoginRequest
    {
        [Required]
        public string? GoogleToken { get; set; }
    }
}
