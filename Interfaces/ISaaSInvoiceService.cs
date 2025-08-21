
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Models;

namespace FixoraBackend.Interfaces
{
    public interface ISaaSInvoiceService
    {
        Task<SaaSInvoice> CreateInvoiceAsync(CreateSaaSInvoiceRequest request);

        Task<SaaSInvoice[]> GetInvoicesByPartnerIdAsync(int partnerId);

        Task<bool> MarkInvoiceAsPaidAsync(int invoiceId);

        Task<bool> DeleteInvoiceAsync(int invoiceId);
    }
}

