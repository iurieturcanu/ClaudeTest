using Microsoft.EntityFrameworkCore;
using PublicServiceRegister.Domain.Entities;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Infrastructure.Persistence.Repositories;

public class ServiceCategoryRepository : RepositoryBase<ServiceCategory>, IServiceCategoryRepository
{
    public ServiceCategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ServiceCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<ServiceCategory>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync(cancellationToken);
    }
}
