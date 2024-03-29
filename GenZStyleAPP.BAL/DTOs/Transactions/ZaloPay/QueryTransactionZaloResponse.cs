using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Transactions.ZaloPay
{
    public class QueryTransactionZaloResponse
    {        
        /*public string? PartnerCode { get; set; }*/
        public string? OrderId { get; set; }
        public string? RequestId { get; set; }
        public string? ExtraData { get; set; }
        public decimal? Amount { get; set; }
        public long? TransId { get; set; }
        public string PayType { get; set; }
        public int ResultCode { get; set; }
        //object refund
        public object? RefundTrans { get; set; }
        public string? Message { get; set; }
        public long? ResponseTime { get; set; }
        public long? LastUpdated { get; set; }
    }
}
