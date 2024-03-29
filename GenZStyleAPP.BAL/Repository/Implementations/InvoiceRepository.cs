using AutoMapper;
using GenZStyleAPP.BAL.DTOs.HashTags;
using GenZStyleAPP.BAL.DTOs.Invoices;
using GenZStyleAPP.BAL.Repository.Interfaces;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public InvoiceRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        #region GetHashTag
        public async Task<GetInvoiceResponse> GetAggregateinvoices()
        {
            try
            {
                var invoices = await _unitOfWork.InvoiceDAO.GetAllInvoice();
                decimal total = invoices.Sum(invoice => invoice.Total);
                GetInvoiceResponse getInvoice = new GetInvoiceResponse {
                    Date = DateTime.Now,
                    Total = total,
                };
                return getInvoice;

                
                
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion

    }
}
