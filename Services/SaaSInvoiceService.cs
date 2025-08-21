
using System;
using System.Linq;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class SaaSInvoiceService : ISaaSInvoiceService
    {
        private readonly AppDbContext _context;

        public SaaSInvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SaaSInvoice> CreateInvoiceAsync(CreateSaaSInvoiceRequest request)
        {
            var invoice = new SaaSInvoice
            {
                SaaSPartnerId = request.SaaSPartnerId,
                ClientName = request.ClientName,
                ClientContact = request.ClientContact,
                Amount = request.Amount,
                Notes = request.Notes,
                IsPaid = false,
                IssuedAt = DateTime.UtcNow
            };

            _context.SaaSInvoices.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }

        public async Task<SaaSInvoice[]> GetInvoicesByPartnerIdAsync(int partnerId)
        {
            return await _context.SaaSInvoices
                .Where(i => i.SaaSPartnerId == partnerId)
                .OrderByDescending(i => i.IssuedAt)
                .ToArrayAsync();
        }

        public async Task<bool> MarkInvoiceAsPaidAsync(int invoiceId)
        {
            var invoice = await _context.SaaSInvoices.FindAsync(invoiceId);
            if (invoice == null) return false;

            invoice.IsPaid = true;
            invoice.PaidAt = DateTime.UtcNow;

            _context.SaaSInvoices.Update(invoice);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteInvoiceAsync(int invoiceId)
        {
            var invoice = await _context.SaaSInvoices.FindAsync(invoiceId);
            if (invoice == null) return false;

            _context.SaaSInvoices.Remove(invoice);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

