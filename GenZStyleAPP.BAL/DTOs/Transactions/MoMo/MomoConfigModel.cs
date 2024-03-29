using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Transactions.MoMo
{
    public class MomoConfigModel
    {
        public string? PayGate { get; set; }
        public string? PartnerCode { get; set; }
        public string? PartnerName { get; set; }
        public string? AccessKey { get; set; }
        public string? SecretKey { get; set; }
        public string? RequestType { get; set; }
        public bool? AutoCapture { get; set; }
        public string? NotifyUrl { get; set; }
        public int? OrderExpireTime { get; set; }
        public string? Lang { get; set; }
        public string? OrderInfo { get; set; }
        public string? ExtraData { get; set; }
    }
}
