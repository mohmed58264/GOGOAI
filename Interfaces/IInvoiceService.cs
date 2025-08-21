
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Models;

namespace FixoraBackend.Interfaces
{
    public interface IInvoiceService
    {
        Task<Invoice> CreateInvoiceAsync(CreateInvoiceRequest request, string createdByUserId);

        Task<bool> ApproveOrRejectInvoiceAsync(ApproveInvoiceRequest request, string customerId);
    }
}

