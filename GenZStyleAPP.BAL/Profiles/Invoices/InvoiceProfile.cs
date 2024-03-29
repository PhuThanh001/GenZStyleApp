using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Profiles.Invoices
{
    public class InvoiceProfile : Profile
    {   
        public InvoiceProfile() 
        {
            CreateMap<Invoice, GetTransactionResponse>().ReverseMap(); 
            CreateMap<Invoice, GetInvoiceResponse>().ReverseMap(); 
        }
        
    }
}
