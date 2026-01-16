using PublicServiceRegister.Models;

namespace PublicServiceRegister.Services;

public interface ICategoryRepository
{
    Task<IEnumerable<ServiceCategory>> GetAllAsync();
    Task<ServiceCategory?> GetByIdAsync(int id);
    Task<ServiceCategory> AddAsync(ServiceCategory category);
    Task<ServiceCategory> UpdateAsync(ServiceCategory category);
    Task<bool> DeleteAsync(int id);
}
