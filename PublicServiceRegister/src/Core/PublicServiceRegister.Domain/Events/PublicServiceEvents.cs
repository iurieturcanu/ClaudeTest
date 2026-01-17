using PublicServiceRegister.Domain.Common;

namespace PublicServiceRegister.Domain.Events;

public sealed class PublicServiceCreatedEvent : DomainEvent
{
    public string Name { get; }
    public Guid CategoryId { get; }

    public PublicServiceCreatedEvent(Guid aggregateId, string name, Guid categoryId) : base(aggregateId)
    {
        Name = name;
        CategoryId = categoryId;
    }
}

public sealed class PublicServiceUpdatedEvent : DomainEvent
{
    public string Name { get; }
    public Guid CategoryId { get; }
    public int Version { get; }

    public PublicServiceUpdatedEvent(Guid aggregateId, string name, Guid categoryId, int version) : base(aggregateId)
    {
        Name = name;
        CategoryId = categoryId;
        Version = version;
    }
}

public sealed class PublicServicePublishedEvent : DomainEvent
{
    public PublicServicePublishedEvent(Guid aggregateId) : base(aggregateId) { }
}

public sealed class PublicServiceUnpublishedEvent : DomainEvent
{
    public PublicServiceUnpublishedEvent(Guid aggregateId) : base(aggregateId) { }
}

public sealed class PublicServiceMarkedForReviewEvent : DomainEvent
{
    public PublicServiceMarkedForReviewEvent(Guid aggregateId) : base(aggregateId) { }
}

public sealed class PublicServiceDeprecatedEvent : DomainEvent
{
    public PublicServiceDeprecatedEvent(Guid aggregateId) : base(aggregateId) { }
}

public sealed class PublicServiceArchivedEvent : DomainEvent
{
    public PublicServiceArchivedEvent(Guid aggregateId) : base(aggregateId) { }
}
