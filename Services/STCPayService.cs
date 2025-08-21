
using FixoraBackend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FixoraBackend.Services
{
    public class STCPayService
    {
        private readonly HttpClient _httpClient;

        public STCPayService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SendPaymentAsync(string phoneNumber, decimal amount, string reference)
        {
            var payload = new
            {
                mobileNumber = phoneNumber,
                amount = amount,
                reference = reference
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // ⚠️ ضع هنا الرابط الفعلي لـ STCPay API
                var response = await _httpClient.PostAsync("https://api.stcpay.com.sa/v1/payments", content);

                if (response.IsSuccessStatusCode)
                    return true;

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine("STCPay Error: " + error);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("STCPay Exception: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdatePayoutStatusAsync(int payoutId, string status)
        {
            var payout = await _context.PayoutRequests.FindAsync(payoutId);
            if (payout == null) return false;

            payout.Status = status;

            if (status == "Paid")
            {
                payout.PaidAt = DateTime.UtcNow;

                // ✅ إذا كانت طريقة الدفع STCPay يتم التحويل تلقائيًا
                if (payout.PaymentMethod == "STCPay")
                {
                    var stcResult = await _stcPayService.SendPaymentAsync(
                        payout.PaymentDetails,
                        payout.Amount,
                        $"Payout-{payout.Id}"
                    );

                    if (!stcResult)
                        throw new InvalidOperationException("فشل تحويل المبلغ عبر STCPay.");
                }
            }

            _context.PayoutRequests.Update(payout);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}





