
using FixoraBackend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/admin/referrals")]
    [Authorize(Roles = "Admin")]
    public class AdminReferralController : ControllerBase
    {
        private readonly IReferralService _referralService;

        public AdminReferralController(IReferralService referralService)
        {
            _referralService = referralService;
        }

        // ⬅️ عرض جميع الإحالات
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReferrals()
        {
            var referrals = await _referralService.GetReferralsByUserAsync(null); // تعديل الخدمة لدعم null للكل
            return Ok(referrals);
        }

        // ⬅️ عرض إجمالي العمولات المستحقة والمدفوعة
        [HttpGet("stats")]
        public async Task<IActionResult> GetReferralStats()
        {
            var allReferrals = await _referralService.GetReferralsByUserAsync(null);
            decimal totalEarned = 0;
            decimal totalPaid = 0;

            foreach (var referral in allReferrals)
            {
                totalEarned += referral.CommissionEarned;
                if (referral.IsCommissionPaid)
                    totalPaid += referral.CommissionEarned;
            }

            return Ok(new
            {
                totalEarned,
                totalPaid,
                pending = totalEarned - totalPaid
            });
        }

        // ⬅️ دفع عمولة إحالة
        [HttpPost("{referralId}/pay")]
        public async Task<IActionResult> PayCommission(int referralId)
        {
            var result = await _referralService.MarkCommissionAsPaidAsync(referralId);
            if (!result) return NotFound();
            return Ok(new { message = "Commission marked as paid" });
        }


        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredReferrals(DateTime? fromDate, DateTime? toDate, bool? isPaid)
        {
            var referrals = await _referralService.GetReferralsByUserAsync(null);

            if (fromDate.HasValue)
                referrals = referrals.Where(r => r.ReferredAt >= fromDate.Value).ToArray();

            if (toDate.HasValue)
                referrals = referrals.Where(r => r.ReferredAt <= toDate.Value).ToArray();

            if (isPaid.HasValue)
                referrals = referrals.Where(r => r.IsCommissionPaid == isPaid.Value).ToArray();

            return Ok(referrals);
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportReferralsToCsv()
        {
            var referrals = await _referralService.GetReferralsByUserAsync(null);

            var sb = new StringBuilder();
            sb.AppendLine("ReferrerUserId,ReferredUserId,ReferredAt,CommissionEarned,IsCommissionPaid");

            foreach (var r in referrals)
            {
                sb.AppendLine($"{r.ReferrerUserId},{r.ReferredUserId},{r.ReferredAt},{r.CommissionEarned},{r.IsCommissionPaid}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "referrals_report.csv");
        }
    }
}

