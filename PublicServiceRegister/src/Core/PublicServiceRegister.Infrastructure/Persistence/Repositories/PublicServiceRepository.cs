using Microsoft.EntityFrameworkCore;
using PublicServiceRegister.Domain.Entities;
using PublicServiceRegister.Domain.Enums;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Infrastructure.Persistence.Repositories;

public class PublicServiceRepository : RepositoryBase<PublicService>, IPublicServiceRepository
{
    public PublicServiceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<PublicService?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Category)
            .Include(s => s.Provider)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<PublicService>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Category)
            .Include(s => s.Provider)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PublicService>> GetByCategoryAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Category)
            .Include(s => s.Provider)
            .Where(s => s.CategoryId == categoryId)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PublicService>> GetByProviderAsync(
        Guid providerId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Category)
            .Include(s => s.Provider)
            .Where(s => s.ProviderId == providerId)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PublicService>> GetByStatusAsync(
        ServiceStatus status,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Category)
            .Include(s => s.Provider)
            .Where(s => s.Status == status)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PublicService>> GetActiveServicesAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Category)
            .Include(s => s.Provider)
            .Where(s => s.Status == ServiceStatus.Active)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PublicService>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        var term = searchTerm.ToLower();
        return await DbSet
            .Include(s => s.Category)
            .Include(s => s.Provider)
            .Where(s => s.Name.ToLower().Contains(term) ||
                       (s.Description != null && s.Description.ToLower().Contains(term)))
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }
}
