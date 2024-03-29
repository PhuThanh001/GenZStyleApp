using GenZStyleAPP.BAL.DTOs.Accounts;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Postss
{
    public class GetPostResponsee
    {
        [Key]

        public int PostId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public string Content { get; set; }
        public string Image { get; set; }
        public GetAccountResponse Account { get; set; }
        

    }
}
