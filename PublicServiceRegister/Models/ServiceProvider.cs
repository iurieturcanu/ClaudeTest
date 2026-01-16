using PublicServiceRegister.Models.Versioning;

namespace PublicServiceRegister.Models;

public class ServiceProvider : IVersionedEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProviderType Type { get; set; } = ProviderType.Government;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Versioning properties
    public int Version { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation property for services provided
    public List<int> ServiceIds { get; set; } = new();
}
