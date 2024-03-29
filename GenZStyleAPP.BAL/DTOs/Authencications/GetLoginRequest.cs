using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParticipantManagement.BAL.DTOs.Authentications
{
    public class GetLoginRequest
    {
        [Key]
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
