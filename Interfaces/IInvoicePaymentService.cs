
using System.Threading.Tasks;
using FixoraBackend.DTOs;

namespace FixoraBackend.Interfaces
{
    public interface IInvoicePaymentService
    {
        Task<bool> CreatePaymentAsync(CreateInvoicePaymentRequest request, string userId);
    }
}

