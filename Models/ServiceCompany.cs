namespace FixoraBackend.Models
{
    public class ServiceCompany
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string CommercialNumber { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool IsVerified { get; set; } = false;

        public bool IsBlocked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}