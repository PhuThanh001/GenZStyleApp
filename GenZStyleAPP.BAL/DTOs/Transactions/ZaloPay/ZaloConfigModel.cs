using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Transactions.ZaloPay
{
    public class ZaloConfigModel
    {
        public string? url { get; set; }
        public string? app_id { get; set; }
        public string? key { get; set; }
        public string? app_user { get; set; }
        public string? description { get; set; }
        public string? bank_code { get; set; }
    }
}
