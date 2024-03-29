using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Transactions.MoMo
{
    public class PostTransactionMomoResponse
    {
        public string? PartnerCode { get; set; }
        public string? OrderId { get; set; }
        public string? RequestId { get; set; }
        public double? Amount { get; set; }
        public long? ResponseTime { get; set; }
        public string? Message { get; set; }
        public int? ResultCode { get; set; }
        public string? PayUrl { get; set; }
        public string? Deeplink { get; set; }
        public string? QrCodeUrl { get; set; }
        public string? Applink { get; set; }
    }
}
