namespace PublicServiceRegister.Domain.Common;

public abstract class AggregateRoot : Entity
{
    public int Version { get; protected set; }

    protected AggregateRoot() : base()
    {
        Version = 0;
    }

    protected AggregateRoot(Guid id) : base(id)
    {
        Version = 0;
    }

    protected void IncrementVersion()
    {
        Version++;
    }

    protected void Apply(DomainEvent @event)
    {
        When(@event);
        IncrementVersion();
        AddDomainEvent(@event);
    }

    protected abstract void When(DomainEvent @event);

    public void LoadFromHistory(IEnumerable<DomainEvent> history)
    {
        foreach (var @event in history)
        {
            When(@event);
            IncrementVersion();
        }
    }
}
