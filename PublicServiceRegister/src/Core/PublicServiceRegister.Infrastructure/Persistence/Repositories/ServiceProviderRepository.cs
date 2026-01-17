using Microsoft.EntityFrameworkCore;
using PublicServiceRegister.Domain.Entities;
using PublicServiceRegister.Domain.Enums;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Infrastructure.Persistence.Repositories;

public class ServiceProviderRepository : RepositoryBase<ServiceProvider>, IServiceProviderRepository
{
    public ServiceProviderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ServiceProvider?> GetByRegistrationNumberAsync(
        string registrationNumber,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            p => p.RegistrationNumber == registrationNumber,
            cancellationToken);
    }

    public async Task<IEnumerable<ServiceProvider>> GetByTypeAsync(
        ProviderType providerType,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.ProviderType == providerType)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ServiceProvider>> GetActiveProvidersAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
}
