using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Package
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal Cost { get; set; }
        public string Image { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public int? IsStatus { get; set; }

        public ICollection<Invoice> Invoices { get; set;}

        public virtual ICollection<PackageRegistration> PackageRegistrations { get;}
    }
}
