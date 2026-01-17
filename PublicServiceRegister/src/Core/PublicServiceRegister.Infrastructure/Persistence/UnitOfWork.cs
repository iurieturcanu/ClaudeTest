using Microsoft.EntityFrameworkCore;
using PublicServiceRegister.Domain.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IEventStore _eventStore;

    public UnitOfWork(ApplicationDbContext context, IEventStore eventStore)
    {
        _context = context;
        _eventStore = eventStore;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregates = _context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        foreach (var aggregate in aggregates)
        {
            var events = aggregate.DomainEvents.ToList();
            var expectedVersion = aggregate.Version - events.Count;

            await _eventStore.SaveEventsAsync(
                aggregate.Id,
                events,
                expectedVersion,
                cancellationToken);

            aggregate.ClearDomainEvents();
        }

        return await _context.SaveChangesAsync(cancellationToken);
    }
}
