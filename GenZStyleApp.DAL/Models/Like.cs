using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Like
    {

        public int PostId { get; set; }
        public int LikeBy { get; set; }

        public bool isLike { get; set; }    
        public Post Post { get; set; }
        public Account Account { get; set; }

    }
}
