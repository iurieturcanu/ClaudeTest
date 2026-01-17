using PublicServiceRegister.Domain.Common;

namespace PublicServiceRegister.Domain.Interfaces;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<DomainEvent> events, int expectedVersion, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default);
    Task<int> GetLatestVersionAsync(Guid aggregateId, CancellationToken cancellationToken = default);
}
