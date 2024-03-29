using GenZStyleAPP.BAL.DTOs.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IInvoiceRepository
    {
        public Task<GetInvoiceResponse> GetAggregateinvoices();
    }
}
