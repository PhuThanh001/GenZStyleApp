using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Hashtag
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<HashPost> HashPosts { get; set; }
    }
}
