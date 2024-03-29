using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Transactions
{
    public class PostTransactionRequest
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string RedirectUrl { get; set; }
    }
}
