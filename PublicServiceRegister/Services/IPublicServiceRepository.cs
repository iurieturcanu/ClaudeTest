using PublicServiceRegister.Models;

namespace PublicServiceRegister.Services;

public interface IPublicServiceRepository
{
    Task<IEnumerable<PublicService>> GetAllAsync();
    Task<PublicService?> GetByIdAsync(int id);
    Task<IEnumerable<PublicService>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<PublicService>> SearchAsync(string searchTerm);
    Task<PublicService> AddAsync(PublicService service);
    Task<PublicService> UpdateAsync(PublicService service);
    Task<bool> DeleteAsync(int id);
}
