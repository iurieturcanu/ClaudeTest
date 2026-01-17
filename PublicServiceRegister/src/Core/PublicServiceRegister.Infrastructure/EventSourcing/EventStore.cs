using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PublicServiceRegister.Domain.Common;
using PublicServiceRegister.Domain.Interfaces;
using PublicServiceRegister.Infrastructure.Persistence;

namespace PublicServiceRegister.Infrastructure.EventSourcing;

public class EventStore : IEventStore
{
    private readonly ApplicationDbContext _context;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public EventStore(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SaveEventsAsync(
        Guid aggregateId,
        IEnumerable<DomainEvent> events,
        int expectedVersion,
        CancellationToken cancellationToken = default)
    {
        var eventsList = events.ToList();
        if (!eventsList.Any()) return;

        var aggregateType = eventsList.First().GetType().DeclaringType?.Name
                           ?? eventsList.First().GetType().Name.Replace("Event", "");

        var currentVersion = await GetAggregateVersionAsync(aggregateId, cancellationToken);

        if (currentVersion != expectedVersion)
        {
            throw new InvalidOperationException(
                $"Concurrency conflict for aggregate {aggregateId}. Expected version {expectedVersion}, but current version is {currentVersion}.");
        }

        var version = expectedVersion;
        foreach (var @event in eventsList)
        {
            version++;
            var storedEvent = new StoredEvent
            {
                Id = @event.EventId,
                AggregateId = aggregateId,
                AggregateType = aggregateType,
                EventType = @event.GetType().Name,
                EventData = JsonSerializer.Serialize(@event, @event.GetType(), JsonOptions),
                OccurredOn = @event.OccurredOn,
                Version = version
            };

            await _context.StoredEvents.AddAsync(storedEvent, cancellationToken);
        }
    }

    public async Task<IEnumerable<DomainEvent>> GetEventsAsync(
        Guid aggregateId,
        CancellationToken cancellationToken = default)
    {
        var storedEvents = await _context.StoredEvents
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.Version)
            .ToListAsync(cancellationToken);

        return storedEvents.Select(DeserializeEvent).Where(e => e != null).Cast<DomainEvent>();
    }

    public async Task<IEnumerable<DomainEvent>> GetEventsAsync(
        Guid aggregateId,
        int fromVersion,
        CancellationToken cancellationToken = default)
    {
        var storedEvents = await _context.StoredEvents
            .Where(e => e.AggregateId == aggregateId && e.Version > fromVersion)
            .OrderBy(e => e.Version)
            .ToListAsync(cancellationToken);

        return storedEvents.Select(DeserializeEvent).Where(e => e != null).Cast<DomainEvent>();
    }

    private async Task<int> GetAggregateVersionAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        var lastEvent = await _context.StoredEvents
            .Where(e => e.AggregateId == aggregateId)
            .OrderByDescending(e => e.Version)
            .FirstOrDefaultAsync(cancellationToken);

        return lastEvent?.Version ?? 0;
    }

    private static DomainEvent? DeserializeEvent(StoredEvent storedEvent)
    {
        var eventType = FindEventType(storedEvent.EventType);
        if (eventType == null) return null;

        return JsonSerializer.Deserialize(storedEvent.EventData, eventType, JsonOptions) as DomainEvent;
    }

    private static Type? FindEventType(string eventTypeName)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == eventTypeName && typeof(DomainEvent).IsAssignableFrom(t));
    }
}
