using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.ChatHistorys
{
    public class GetChatHistoryResponse
    {
       
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
