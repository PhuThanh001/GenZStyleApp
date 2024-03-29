using GenZStyleAPP.BAL.DTOs.Invoices;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GenZStyleApp_API.Controllers
{
    
    public class InvoicesController : ODataController
    {       
        public IInvoiceRepository _InvoiceRepository { get; set; }

        public InvoicesController(IInvoiceRepository invoiceRepository) 
        {
            _InvoiceRepository = invoiceRepository;
        }


        [HttpGet("odata/Invoices/GetAggregateinvoices")]
        [EnableQuery]
        public async Task<IActionResult> GetAggregateinvoices()
        {
            try{
                  GetInvoiceResponse Invoice = await this._InvoiceRepository.GetAggregateinvoices().ConfigureAwait(false);
                  return Ok(Invoice);   
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
