using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.DTOs.PostLike;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Posts
{
    public class GetCollectionPostResponse
    {
        [Key]
        public int PostId { get; set; }

        public int AccountId { get; set; }


        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public string Content { get; set; }
        public string Image { get; set; }
        //public ICollection<GetHashPostCollectionResponse> HashPosts { get; set; } //"lưu ý chỗ này cần thì mở ra"

        //public GetAccountResponse Account { get; set; }
        public ICollection<GetCommentResponse> Comments { get; set; }
        public List<string>? Hashtags { get; set; }
        public ICollection<GetPostLikeCollection> Likes { get; set; }
    }
}
