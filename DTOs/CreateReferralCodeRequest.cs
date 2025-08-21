
using FixoraBackend.DTOs;
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CreateReferralCodeRequest
    {
        [Required]
        public string ReferredUserId { get; set; }

        [Required]
        public string ReferrerCode { get; set; }
    }
}



    public class ReferralStatusResponse
    {
        public bool IsReferred { get; set; }

        public string ReferredByUserId { get; set; }

        public decimal TotalEarnings { get; set; }

        public int TotalReferredUsers { get; set; }
    }
