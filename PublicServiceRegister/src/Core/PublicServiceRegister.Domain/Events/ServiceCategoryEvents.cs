using PublicServiceRegister.Domain.Common;

namespace PublicServiceRegister.Domain.Events;

public sealed class ServiceCategoryCreatedEvent : DomainEvent
{
    public string Name { get; }
    public string Description { get; }

    public ServiceCategoryCreatedEvent(Guid aggregateId, string name, string description) : base(aggregateId)
    {
        Name = name;
        Description = description;
    }
}

public sealed class ServiceCategoryUpdatedEvent : DomainEvent
{
    public string Name { get; }
    public string Description { get; }
    public int Version { get; }

    public ServiceCategoryUpdatedEvent(Guid aggregateId, string name, string description, int version) : base(aggregateId)
    {
        Name = name;
        Description = description;
        Version = version;
    }
}

public sealed class ServiceCategoryActivatedEvent : DomainEvent
{
    public ServiceCategoryActivatedEvent(Guid aggregateId) : base(aggregateId) { }
}

public sealed class ServiceCategoryDeactivatedEvent : DomainEvent
{
    public ServiceCategoryDeactivatedEvent(Guid aggregateId) : base(aggregateId) { }
}
