using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace GenZStyleApp.DAL.Models
{
    public class PackageRegistration
    {
        /*[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }*/

        public int AccountId { get; set; }

        public int PackageId { get; set; }
        public DateTime RegistrationDate { get; set; }
        [ForeignKey("AccountId")]

        public Account Account { get; set; }
        [ForeignKey("PackageId")]

        public Package Package { get; set; }

    }
}
