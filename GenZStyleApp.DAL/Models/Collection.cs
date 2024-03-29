using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Collection
    {
        public int Id { get; set; }

        [ForeignKey("AccountId")]
        public int AccountId { get; set; }        
        public Account? Account { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        
        public int PostId { get; set; }

        public bool IsSaved {  get; set; }
        public Category? Category { get; set; }                
        public string Name { get; set; }
        public string Image_url { get; set; }
        public int Type { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
