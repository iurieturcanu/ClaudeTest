namespace PublicServiceRegister.Infrastructure.EventSourcing;

public class StoredEvent
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string AggregateType { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty;
    public DateTime OccurredOn { get; set; }
    public int Version { get; set; }
    public string? UserId { get; set; }
    public string? CorrelationId { get; set; }
}
