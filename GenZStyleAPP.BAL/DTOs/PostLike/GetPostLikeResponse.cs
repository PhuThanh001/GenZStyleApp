using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.Posts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.PostLike
{
    public class GetPostLikeResponse
    {
        [Key]
        public int PostId { get; set; }
        public int LikeBy { get; set; }

        public bool isLike { get; set; }
        public GetAccountResponse Account { get; set; }

    }
}
