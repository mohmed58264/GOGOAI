namespace FixoraBackend.DTOs.DTOs
{
    public class BusinessSiteResponseDto
    {
        public Guid Id { get; set; }
        public Guid BusinessClientId { get; set; }
        public string SiteName { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
