using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class InboxPaticipant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int InboxPaticipantId { get; set; }

        public int InboxId { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }
        
        public Inbox Inbox { get; set; }
    }
}
