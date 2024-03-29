using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.DTOs.PostLike;
using GenZStyleAPP.BAL.DTOs.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Collections
{
    public class GetCollectionResponse
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        /*public Account Account { get; set; }*/
        public int CategoryId { get; set; }
        /*public Category Category { get; set; }*/
        public int PostId { get; set; }
        public GetCollectionPostResponse Post { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int Type { get; set; }
        public bool IsSaved { get; set; }
        public List<string>? Hashtags { get; set; }  // thêm zô chỗ này 

        public ICollection<GetPostLikeCollection> Likes { get; set; }
        public ICollection<GetCommentResponse> Comments { get; set; }
    }
}
