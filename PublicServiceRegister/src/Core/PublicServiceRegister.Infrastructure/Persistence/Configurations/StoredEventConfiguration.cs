using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PublicServiceRegister.Infrastructure.EventSourcing;

namespace PublicServiceRegister.Infrastructure.Persistence.Configurations;

public class StoredEventConfiguration : IEntityTypeConfiguration<StoredEvent>
{
    public void Configure(EntityTypeBuilder<StoredEvent> builder)
    {
        builder.ToTable("StoredEvents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AggregateId)
            .IsRequired();

        builder.Property(x => x.AggregateType)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.EventData)
            .IsRequired();

        builder.Property(x => x.OccurredOn)
            .IsRequired();

        builder.Property(x => x.Version)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasMaxLength(256);

        builder.Property(x => x.CorrelationId)
            .HasMaxLength(100);

        builder.HasIndex(x => x.AggregateId);

        builder.HasIndex(x => x.AggregateType);

        builder.HasIndex(x => x.OccurredOn);

        builder.HasIndex(x => new { x.AggregateId, x.Version })
            .IsUnique();
    }
}
