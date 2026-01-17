using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PublicServiceRegister.Domain.Entities;
using PublicServiceRegister.Infrastructure.EventSourcing;
using PublicServiceRegister.Infrastructure.Identity;
using PublicServiceRegister.Infrastructure.Persistence.Configurations;

namespace PublicServiceRegister.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<PublicServiceProvider> PublicServiceProviders => Set<PublicServiceProvider>();
    public DbSet<PublicService> PublicServices => Set<PublicService>();
    public DbSet<StoredEvent> StoredEvents => Set<StoredEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ServiceCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new PublicServiceProviderConfiguration());
        modelBuilder.ApplyConfiguration(new PublicServiceConfiguration());
        modelBuilder.ApplyConfiguration(new StoredEventConfiguration());

        // Rename Identity tables
        modelBuilder.Entity<ApplicationUser>(b => b.ToTable("Users"));
        modelBuilder.Entity<IdentityRole<Guid>>(b => b.ToTable("Roles"));
        modelBuilder.Entity<IdentityUserRole<Guid>>(b => b.ToTable("UserRoles"));
        modelBuilder.Entity<IdentityUserClaim<Guid>>(b => b.ToTable("UserClaims"));
        modelBuilder.Entity<IdentityUserLogin<Guid>>(b => b.ToTable("UserLogins"));
        modelBuilder.Entity<IdentityRoleClaim<Guid>>(b => b.ToTable("RoleClaims"));
        modelBuilder.Entity<IdentityUserToken<Guid>>(b => b.ToTable("UserTokens"));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update timestamps
        foreach (var entry in ChangeTracker.Entries<Domain.Common.Entity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
