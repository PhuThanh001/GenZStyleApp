using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Message
    {

        public int AccountId { get; set; }

        public int InboxId { get; set; }

        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public Boolean Seen { get; set; }

        public string Content { get; set; }

        public string File { get; set; }

        public DateTime CreateAt {  get; set; }   
        
        public DateTime? DeleteAt {  get; set; }

        public Inbox Inbox { get; set; }

        public Account AccountSender { get; set; }
        
        
    }
}
