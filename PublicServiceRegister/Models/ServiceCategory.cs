using PublicServiceRegister.Models.Versioning;

namespace PublicServiceRegister.Models;

public class ServiceCategory : IVersionedEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconClass { get; set; } = "bi bi-folder";

    // Versioning properties
    public int Version { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? ModifiedBy { get; set; }
}
