using GenZStyleAPP.BAL.DTOs.Posts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.HashPosts
{
    public class GetHashPostByHashtag
    {
        public int PostId { get; set; }
        [Key]
        public int HashTageId { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
        public GetCollectionPostResponse Post { get; set; } // lưu ý chỗ này 
    }
}
