using PublicServiceRegister.Domain.Common;
using PublicServiceRegister.Domain.Enums;
using PublicServiceRegister.Domain.Events;

namespace PublicServiceRegister.Domain.Entities;

public class PublicService : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid CategoryId { get; private set; }
    public Guid? ServiceProviderId { get; private set; }
    public string ContactEmail { get; private set; } = string.Empty;
    public string ContactPhone { get; private set; } = string.Empty;
    public string Website { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public ServiceStatus Status { get; private set; }
    public string OperatingHours { get; private set; } = string.Empty;
    public decimal? Fee { get; private set; }
    public string FeeDescription { get; private set; } = string.Empty;

    private readonly List<string> _requiredDocuments = new();
    public IReadOnlyCollection<string> RequiredDocuments => _requiredDocuments.AsReadOnly();

    private PublicService() : base() { }

    public static PublicService Create(
        string name,
        string description,
        Guid categoryId,
        Guid? serviceProviderId = null,
        string? contactEmail = null,
        string? contactPhone = null,
        string? website = null,
        string? address = null,
        string? operatingHours = null,
        decimal? fee = null,
        string? feeDescription = null,
        IEnumerable<string>? requiredDocuments = null,
        string? createdBy = null)
    {
        var service = new PublicService
        {
            Name = name,
            Description = description,
            CategoryId = categoryId,
            ServiceProviderId = serviceProviderId,
            ContactEmail = contactEmail ?? string.Empty,
            ContactPhone = contactPhone ?? string.Empty,
            Website = website ?? string.Empty,
            Address = address ?? string.Empty,
            Status = ServiceStatus.Draft,
            OperatingHours = operatingHours ?? string.Empty,
            Fee = fee,
            FeeDescription = feeDescription ?? string.Empty,
            CreatedBy = createdBy
        };

        if (requiredDocuments != null)
        {
            service._requiredDocuments.AddRange(requiredDocuments);
        }

        service.Apply(new PublicServiceCreatedEvent(service.Id, name, categoryId));
        return service;
    }

    public void Update(
        string name,
        string description,
        Guid categoryId,
        Guid? serviceProviderId,
        string? contactEmail,
        string? contactPhone,
        string? website,
        string? address,
        string? operatingHours,
        decimal? fee,
        string? feeDescription,
        IEnumerable<string>? requiredDocuments,
        string? updatedBy = null)
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
        ServiceProviderId = serviceProviderId;
        ContactEmail = contactEmail ?? string.Empty;
        ContactPhone = contactPhone ?? string.Empty;
        Website = website ?? string.Empty;
        Address = address ?? string.Empty;
        OperatingHours = operatingHours ?? string.Empty;
        Fee = fee;
        FeeDescription = feeDescription ?? string.Empty;

        _requiredDocuments.Clear();
        if (requiredDocuments != null)
        {
            _requiredDocuments.AddRange(requiredDocuments);
        }

        SetUpdated(updatedBy);
        Apply(new PublicServiceUpdatedEvent(Id, name, categoryId, Version + 1));
    }

    public void Publish(string? updatedBy = null)
    {
        if (Status == ServiceStatus.Active) return;
        Status = ServiceStatus.Active;
        SetUpdated(updatedBy);
        Apply(new PublicServicePublishedEvent(Id));
    }

    public void Unpublish(string? updatedBy = null)
    {
        Status = ServiceStatus.Inactive;
        SetUpdated(updatedBy);
        Apply(new PublicServiceUnpublishedEvent(Id));
    }

    public void MarkForReview(string? updatedBy = null)
    {
        Status = ServiceStatus.UnderReview;
        SetUpdated(updatedBy);
        Apply(new PublicServiceMarkedForReviewEvent(Id));
    }

    public void Deprecate(string? updatedBy = null)
    {
        Status = ServiceStatus.Deprecated;
        SetUpdated(updatedBy);
        Apply(new PublicServiceDeprecatedEvent(Id));
    }

    public void Archive(string? updatedBy = null)
    {
        Status = ServiceStatus.Archived;
        SetUpdated(updatedBy);
        Apply(new PublicServiceArchivedEvent(Id));
    }

    public void AddRequiredDocument(string document)
    {
        if (!_requiredDocuments.Contains(document))
        {
            _requiredDocuments.Add(document);
        }
    }

    public void RemoveRequiredDocument(string document)
    {
        _requiredDocuments.Remove(document);
    }

    protected override void When(DomainEvent @event)
    {
        switch (@event)
        {
            case PublicServiceCreatedEvent e:
                Id = e.AggregateId;
                Name = e.Name;
                CategoryId = e.CategoryId;
                Status = ServiceStatus.Draft;
                break;
            case PublicServiceUpdatedEvent e:
                Name = e.Name;
                CategoryId = e.CategoryId;
                break;
            case PublicServicePublishedEvent:
                Status = ServiceStatus.Active;
                break;
            case PublicServiceUnpublishedEvent:
                Status = ServiceStatus.Inactive;
                break;
            case PublicServiceMarkedForReviewEvent:
                Status = ServiceStatus.UnderReview;
                break;
            case PublicServiceDeprecatedEvent:
                Status = ServiceStatus.Deprecated;
                break;
            case PublicServiceArchivedEvent:
                Status = ServiceStatus.Archived;
                break;
        }
    }
}
