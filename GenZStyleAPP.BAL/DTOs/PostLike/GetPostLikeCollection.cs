using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.PostLike
{
    public class GetPostLikeCollection
    {
        [Key]
        public int PostId { get; set; }
        public int LikeBy { get; set; }

        public bool isLike { get; set; }
    }
}
