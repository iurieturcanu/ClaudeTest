using PublicServiceRegister.Domain.Common;
using PublicServiceRegister.Domain.Enums;

namespace PublicServiceRegister.Domain.Events;

public sealed class ServiceProviderCreatedEvent : DomainEvent
{
    public string Name { get; }
    public ProviderType Type { get; }

    public ServiceProviderCreatedEvent(Guid aggregateId, string name, ProviderType type) : base(aggregateId)
    {
        Name = name;
        Type = type;
    }
}

public sealed class ServiceProviderUpdatedEvent : DomainEvent
{
    public string Name { get; }
    public ProviderType Type { get; }
    public int Version { get; }

    public ServiceProviderUpdatedEvent(Guid aggregateId, string name, ProviderType type, int version) : base(aggregateId)
    {
        Name = name;
        Type = type;
        Version = version;
    }
}

public sealed class ServiceProviderActivatedEvent : DomainEvent
{
    public ServiceProviderActivatedEvent(Guid aggregateId) : base(aggregateId) { }
}

public sealed class ServiceProviderDeactivatedEvent : DomainEvent
{
    public ServiceProviderDeactivatedEvent(Guid aggregateId) : base(aggregateId) { }
}
