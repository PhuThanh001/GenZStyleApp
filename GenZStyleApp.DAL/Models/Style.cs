using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Style
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int StyleId { get; set; }

        public int AccountId { get; set; }

        public int CategoryId { get; set; }
        
        public string StyleName { get; set; }

        public string Description { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }


    }
}
