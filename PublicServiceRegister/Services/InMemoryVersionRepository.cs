using PublicServiceRegister.Models.Versioning;

namespace PublicServiceRegister.Services;

public class InMemoryVersionRepository : IVersionRepository
{
    private readonly List<EntityVersion> _versions = new();
    private int _nextId = 1;

    public Task<EntityVersion> SaveVersionAsync<T>(T entity, int entityId, int versionNumber, string? modifiedBy = null, string? changeDescription = null) where T : class
    {
        var version = EntityVersion.Create(entity, entityId, versionNumber, modifiedBy, changeDescription);
        version.Id = _nextId++;
        _versions.Add(version);
        return Task.FromResult(version);
    }

    public Task<IEnumerable<EntityVersion>> GetVersionHistoryAsync(string entityType, int entityId)
    {
        var history = _versions
            .Where(v => v.EntityType == entityType && v.EntityId == entityId)
            .OrderByDescending(v => v.VersionNumber)
            .ToList();

        return Task.FromResult<IEnumerable<EntityVersion>>(history);
    }

    public Task<EntityVersion?> GetVersionAsync(string entityType, int entityId, int versionNumber)
    {
        var version = _versions
            .FirstOrDefault(v => v.EntityType == entityType && v.EntityId == entityId && v.VersionNumber == versionNumber);

        return Task.FromResult(version);
    }

    public Task<T?> RestoreVersionAsync<T>(string entityType, int entityId, int versionNumber) where T : class
    {
        var version = _versions
            .FirstOrDefault(v => v.EntityType == entityType && v.EntityId == entityId && v.VersionNumber == versionNumber);

        return Task.FromResult(version?.GetEntity<T>());
    }

    public Task<int> GetLatestVersionNumberAsync(string entityType, int entityId)
    {
        var latestVersion = _versions
            .Where(v => v.EntityType == entityType && v.EntityId == entityId)
            .OrderByDescending(v => v.VersionNumber)
            .FirstOrDefault();

        return Task.FromResult(latestVersion?.VersionNumber ?? 0);
    }
}
