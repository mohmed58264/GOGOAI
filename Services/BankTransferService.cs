
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FixoraBackend.Services
{
    public class BankTransferService
    {
        // إنشاء ملف تحويل بنكي بصيغة CSV أو TXT لإرساله للبنك
        public async Task<string> GenerateBankTransferFileAsync(string accountNumber, decimal amount, string beneficiaryName, string reference)
        {
            var fileName = $"BankTransfer_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            var csvContent = new StringBuilder();
            csvContent.AppendLine("AccountNumber,BeneficiaryName,Amount,Reference");
            csvContent.AppendLine($"{accountNumber},{beneficiaryName},{amount},{reference}");

            await File.WriteAllTextAsync(filePath, csvContent.ToString(), Encoding.UTF8);

            return filePath; // مسار الملف الذي تم إنشاؤه
        }

        // تنفيذ التحويل البنكي عبر API البنك إذا كان متاح
        public async Task<bool> SendBankTransferAsync(string accountNumber, decimal amount, string beneficiaryName, string reference)
        {
            try
            {
                // ⚠️ ضع هنا كود الاتصال بـ API البنك الفعلي
                Console.WriteLine($"تحويل بنكي إلى {beneficiaryName} - رقم الحساب: {accountNumber} - مبلغ: {amount} - مرجع: {reference}");
                await Task.Delay(500); // محاكاة الاتصال بالبنك
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bank Transfer Error: " + ex.Message);
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

                // ✅ إذا كانت طريقة الدفع STCPay
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

                // ✅ إذا كانت طريقة الدفع تحويل بنكي
                else if (payout.PaymentMethod == "BankTransfer")
                {
                    var bankResult = await _bankTransferService.SendBankTransferAsync(
                        payout.PaymentDetails, // رقم الحساب البنكي
                        payout.Amount,
                        "Beneficiary Name", // يمكن جلبه من بيانات المستخدم
                        $"Payout-{payout.Id}"
                    );

                    if (!bankResult)
                        throw new InvalidOperationException("فشل التحويل البنكي.");
                }
            }

            _context.PayoutRequests.Update(payout);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

