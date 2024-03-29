using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int CategoryId { get; set; }
        public int PostId { get; set; }
        public string CategoryName { get; set; }
        
        public string CategoryDescription { get; set; }
        
        public Style Style { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        
    }
}
