using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Comments
{
    public class GetAllCommentResponse
    {
        public int CommentId { get; set; }

        public int PostId { get; set; }

        public DateTime CreateAt { get; set; }
        public string Content { get; set; }

        public int CommentBy { get; set; }
        public Post Post { get; set; }
    }
}
