using PublicServiceRegister.Models;

namespace PublicServiceRegister.Services;

public interface IServiceProviderRepository
{
    Task<IEnumerable<ServiceProvider>> GetAllAsync();
    Task<ServiceProvider?> GetByIdAsync(int id);
    Task<IEnumerable<ServiceProvider>> GetByTypeAsync(ProviderType type);
    Task<IEnumerable<ServiceProvider>> SearchAsync(string searchTerm);
    Task<ServiceProvider> AddAsync(ServiceProvider provider);
    Task<ServiceProvider> UpdateAsync(ServiceProvider provider, string? modifiedBy = null, string? changeDescription = null);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ServiceProvider>> GetActiveProvidersAsync();
}
