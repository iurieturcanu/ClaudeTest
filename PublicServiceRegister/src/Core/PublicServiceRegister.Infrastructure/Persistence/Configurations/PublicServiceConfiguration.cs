using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PublicServiceRegister.Domain.Entities;
using PublicServiceRegister.Domain.Enums;

namespace PublicServiceRegister.Infrastructure.Persistence.Configurations;

public class PublicServiceConfiguration : IEntityTypeConfiguration<PublicService>
{
    public void Configure(EntityTypeBuilder<PublicService> builder)
    {
        builder.ToTable("PublicServices");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Description)
            .HasMaxLength(4000);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.Requirements)
            .HasMaxLength(4000);

        builder.Property(x => x.Fees)
            .HasMaxLength(1000);

        builder.Property(x => x.ProcessingTime)
            .HasMaxLength(200);

        builder.Property(x => x.ContactInfo)
            .HasMaxLength(500);

        builder.Property(x => x.OnlineServiceUrl)
            .HasMaxLength(500);

        builder.Property(x => x.Version)
            .IsRequired()
            .IsConcurrencyToken();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.PublishedAt);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Provider)
            .WithMany()
            .HasForeignKey(x => x.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Name);

        builder.HasIndex(x => x.CategoryId);

        builder.HasIndex(x => x.ProviderId);

        builder.HasIndex(x => x.Status);

        builder.Ignore(x => x.DomainEvents);
    }
}
