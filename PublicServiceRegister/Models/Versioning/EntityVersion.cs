using System.Text.Json;

namespace PublicServiceRegister.Models.Versioning;

public class EntityVersion
{
    public int Id { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public int VersionNumber { get; set; }
    public string SerializedData { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ModifiedBy { get; set; }
    public string? ChangeDescription { get; set; }

    public T? GetEntity<T>() where T : class
    {
        if (string.IsNullOrEmpty(SerializedData))
            return null;

        return JsonSerializer.Deserialize<T>(SerializedData);
    }

    public static EntityVersion Create<T>(T entity, int entityId, int versionNumber, string? modifiedBy = null, string? changeDescription = null) where T : class
    {
        return new EntityVersion
        {
            EntityType = typeof(T).Name,
            EntityId = entityId,
            VersionNumber = versionNumber,
            SerializedData = JsonSerializer.Serialize(entity),
            CreatedAt = DateTime.UtcNow,
            ModifiedBy = modifiedBy,
            ChangeDescription = changeDescription
        };
    }
}
