using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Invoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int InvoiceId { get; set; }
        
        public string RechargeID { get; set; }
        public int? AccountId { get; set; }
        public int? WalletId { get; set; }
        public int? PackageId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
        public string? PaymentType { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        [ForeignKey("PackageId")]
        public Package Package { get; set; }
        
        

        
        
    }
}
