using PublicServiceRegister.Models.Versioning;

namespace PublicServiceRegister.Services;

public interface IVersionRepository
{
    Task<EntityVersion> SaveVersionAsync<T>(T entity, int entityId, int versionNumber, string? modifiedBy = null, string? changeDescription = null) where T : class;
    Task<IEnumerable<EntityVersion>> GetVersionHistoryAsync(string entityType, int entityId);
    Task<EntityVersion?> GetVersionAsync(string entityType, int entityId, int versionNumber);
    Task<T?> RestoreVersionAsync<T>(string entityType, int entityId, int versionNumber) where T : class;
    Task<int> GetLatestVersionNumberAsync(string entityType, int entityId);
}
