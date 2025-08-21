
using System;
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CreateBusinessContractRequest
    {
        [Required]
        public int BusinessClientId { get; set; }

        [Required]
        public string ContractType { get; set; } // Annual, Quarterly, OnDemand

        public decimal? FixedAmountPerPeriod { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Notes { get; set; }
    }
}

