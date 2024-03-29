using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class InvoiceDAO
    {
        private GenZStyleDbContext _dbContext;
        public InvoiceDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region AllHashtag
        public async Task<List<Invoice>> GetAllInvoice()
        {
            return await _dbContext.Invoices.Include(i => i.Package)                                       
                                            .ToListAsync();

        }
        #endregion

        #region Create wallet transaction
        public async Task CreateInvoiceAsync(Invoice invoice)
        {
            try
            {
                await this._dbContext.Invoices.AddAsync(invoice);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task<Invoice> GetInvoiceByRechargeId (string rechargeId)
        {
            try 
            {
                return await _dbContext.Invoices.Where(I => I.RechargeID == rechargeId)
                                                .Include(I => I.Account).ThenInclude(a => a.User).ThenInclude(u => u.Role)                                
                                                .FirstOrDefaultAsync(); 
            }
            catch (Exception ex) 
            {
                throw new Exception (ex.Message);
            }
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            try 
            {
                this._dbContext.Entry<Invoice>(invoice).State = EntityState.Modified;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
