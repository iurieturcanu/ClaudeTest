namespace PublicServiceRegister.Domain.Common;

public abstract class DomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public Guid AggregateId { get; }
    public string EventType => GetType().Name;

    protected DomainEvent(Guid aggregateId)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        AggregateId = aggregateId;
    }
}
