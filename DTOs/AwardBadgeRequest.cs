
using System;
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class AwardBadgeRequest
    {
        [Required]
        public string ProviderUserId { get; set; }

        [Required]
        public string BadgeName { get; set; } // FastResponder, TopRated, MostOrders

        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}
