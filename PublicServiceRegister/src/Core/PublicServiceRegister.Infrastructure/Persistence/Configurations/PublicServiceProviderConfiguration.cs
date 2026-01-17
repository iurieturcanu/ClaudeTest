using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PublicServiceRegister.Domain.Entities;

namespace PublicServiceRegister.Infrastructure.Persistence.Configurations;

public class PublicServiceProviderConfiguration : IEntityTypeConfiguration<PublicServiceProvider>
{
    public void Configure(EntityTypeBuilder<PublicServiceProvider> builder)
    {
        builder.ToTable("PublicServiceProviders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.ContactEmail)
            .HasMaxLength(256);

        builder.Property(x => x.ContactPhone)
            .HasMaxLength(50);

        builder.Property(x => x.Address)
            .HasMaxLength(500);

        builder.Property(x => x.Website)
            .HasMaxLength(500);

        builder.Property(x => x.LogoUrl)
            .HasMaxLength(1000);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.Version)
            .IsRequired()
            .IsConcurrencyToken();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.HasIndex(x => x.Name);

        builder.HasIndex(x => x.Type);

        builder.HasIndex(x => x.IsActive);

        builder.Ignore(x => x.DomainEvents);
    }
}
