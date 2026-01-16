using PublicServiceRegister.Models;
using PublicServiceRegister.Models.Versioning;

namespace PublicServiceRegister.Services;

public interface IPublicServiceRepository
{
    Task<IEnumerable<PublicService>> GetAllAsync();
    Task<PublicService?> GetByIdAsync(int id);
    Task<IEnumerable<PublicService>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<PublicService>> GetByProviderAsync(int providerId);
    Task<IEnumerable<PublicService>> SearchAsync(string searchTerm);
    Task<PublicService> AddAsync(PublicService service);
    Task<PublicService> UpdateAsync(PublicService service, string? modifiedBy = null, string? changeDescription = null);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<EntityVersion>> GetVersionHistoryAsync(int serviceId);
    Task<PublicService?> GetVersionAsync(int serviceId, int versionNumber);
}
