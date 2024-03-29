using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Transactions
{
    public class GetInvoiceResponse
    {
        [Key]
        public int InvoiceId { get; set; }

        public string RechargeID { get; set; }
        public int? AccountId { get; set; }
        public int? WalletId { get; set; }
        public int? PackageId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
    }
}
