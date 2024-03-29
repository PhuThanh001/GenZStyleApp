using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Authencications
{
    public class AuthenticationResponse
    {
        public string? AccessToken { get; set; }

        public string? Role { get; set; }

        public string? RefreshToken { get; set; }
    }
}
