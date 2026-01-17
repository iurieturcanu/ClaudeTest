using Microsoft.EntityFrameworkCore;
using PublicServiceRegister.Domain.Entities;
using PublicServiceRegister.Domain.Enums;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Infrastructure.Persistence.Repositories;

public class PublicServiceProviderRepository : RepositoryBase<PublicServiceProvider>, IPublicServiceProviderRepository
{
    public PublicServiceProviderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PublicServiceProvider?> GetByRegistrationNumberAsync(
        string registrationNumber,
        CancellationToken cancellationToken = default)
    {
        // Note: RegistrationNumber property doesn't exist on the entity yet
        // This is a placeholder for future implementation
        return await DbSet.FirstOrDefaultAsync(
            p => p.Name == registrationNumber,
            cancellationToken);
    }

    public async Task<IEnumerable<PublicServiceProvider>> GetByTypeAsync(
        ProviderType providerType,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.Type == providerType)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PublicServiceProvider>> GetActiveProvidersAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
}
