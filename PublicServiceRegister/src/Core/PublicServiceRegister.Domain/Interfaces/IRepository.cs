using System.Linq.Expressions;
using PublicServiceRegister.Domain.Common;

namespace PublicServiceRegister.Domain.Interfaces;

public interface IRepository<T> where T : AggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IServiceCategoryRepository : IRepository<Entities.ServiceCategory>
{
    Task<Entities.ServiceCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.ServiceCategory>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
}

public interface IPublicServiceProviderRepository : IRepository<Entities.PublicServiceProvider>
{
    Task<Entities.PublicServiceProvider?> GetByRegistrationNumberAsync(string registrationNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.PublicServiceProvider>> GetByTypeAsync(Enums.ProviderType providerType, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.PublicServiceProvider>> GetActiveProvidersAsync(CancellationToken cancellationToken = default);
}

public interface IPublicServiceRepository : IRepository<Entities.PublicService>
{
    Task<IEnumerable<Entities.PublicService>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.PublicService>> GetByProviderAsync(Guid providerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.PublicService>> GetByStatusAsync(Enums.ServiceStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.PublicService>> GetActiveServicesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.PublicService>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
