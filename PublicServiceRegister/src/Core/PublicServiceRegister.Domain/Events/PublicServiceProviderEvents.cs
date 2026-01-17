using PublicServiceRegister.Domain.Common;
using PublicServiceRegister.Domain.Enums;

namespace PublicServiceRegister.Domain.Events;

public sealed class PublicServiceProviderCreatedEvent : DomainEvent
{
    public string Name { get; }
    public ProviderType Type { get; }

    public PublicServiceProviderCreatedEvent(Guid aggregateId, string name, ProviderType type) : base(aggregateId)
    {
        Name = name;
        Type = type;
    }
}

public sealed class PublicServiceProviderUpdatedEvent : DomainEvent
{
    public string Name { get; }
    public ProviderType Type { get; }
    public int Version { get; }

    public PublicServiceProviderUpdatedEvent(Guid aggregateId, string name, ProviderType type, int version) : base(aggregateId)
    {
        Name = name;
        Type = type;
        Version = version;
    }
}

public sealed class PublicServiceProviderActivatedEvent : DomainEvent
{
    public PublicServiceProviderActivatedEvent(Guid aggregateId) : base(aggregateId) { }
}

public sealed class PublicServiceProviderDeactivatedEvent : DomainEvent
{
    public PublicServiceProviderDeactivatedEvent(Guid aggregateId) : base(aggregateId) { }
}
