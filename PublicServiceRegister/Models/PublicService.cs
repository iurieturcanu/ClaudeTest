namespace PublicServiceRegister.Models;

public class PublicService
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public ServiceCategory? Category { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ServiceStatus Status { get; set; } = ServiceStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string OperatingHours { get; set; } = string.Empty;
    public List<string> RequiredDocuments { get; set; } = new();
    public decimal? Fee { get; set; }
    public string FeeDescription { get; set; } = string.Empty;
}
