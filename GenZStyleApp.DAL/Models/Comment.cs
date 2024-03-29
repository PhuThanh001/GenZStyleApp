using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int CommentId { get; set; }

        public int? ParentCommentId { get; set; }
        public int PostId { get; set; }

        public DateTime CreateAt { get; set; }
        public string Content { get; set; }

        public int CommentBy {  get; set; }
        public Post Post { get; set; }
        
        
        public Comment? ParentComment { get; set; }
        public ICollection<Comment>? SubComments { get; set; }

        public Account Account { get; set; }


    }
}
