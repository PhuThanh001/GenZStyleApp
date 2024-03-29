using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int PostId { get; set; }

        public int AccountId { get; set; }

        
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        
        public int TotalLike {  get; set; }

        public int TotalComment { get; set; }
        public string Content { get; set; }
        public string Image {  get; set; }

        public bool Status { get; set; }
        public Account Account { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Like> Likes { get; set;}
        public ICollection<HashPost> HashPosts { get; set;}
        
        public ICollection<Comment> Comments { get;}

        public ICollection<Report> Reports { get; set; }
        public Collection Collection { get; set; }


    }
}
