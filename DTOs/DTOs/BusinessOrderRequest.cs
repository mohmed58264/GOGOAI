public class BusinessOrderRequest
{
    public Guid BusinessClientId { get; set; }
    public Guid BusinessSiteId { get; set; }
    public string ServiceType { get; set; }
    public double Price { get; set; }
}
