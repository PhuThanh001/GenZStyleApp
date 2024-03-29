using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Comments
{
    public class GetCommentRequest
    {
        
        public DateTime CreateAt { get; set; }
        public string Content { get; set; }


    }
}
