using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Inbox
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int InboxId { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; }

        public Account Account { get; set; }
        
        public ICollection<Message> Messages { get; set; }
        public ICollection<InboxPaticipant> Paticipants { get; set; }

        
    }
}
