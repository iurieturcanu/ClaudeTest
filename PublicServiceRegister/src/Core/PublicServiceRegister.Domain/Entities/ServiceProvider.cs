using PublicServiceRegister.Domain.Common;
using PublicServiceRegister.Domain.Enums;
using PublicServiceRegister.Domain.Events;

namespace PublicServiceRegister.Domain.Entities;

public class ServiceProvider : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ProviderType Type { get; private set; }
    public string ContactEmail { get; private set; } = string.Empty;
    public string ContactPhone { get; private set; } = string.Empty;
    public string Website { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string? LogoUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    private ServiceProvider() : base() { }

    public static ServiceProvider Create(
        string name,
        string description,
        ProviderType type,
        string? contactEmail = null,
        string? contactPhone = null,
        string? website = null,
        string? address = null,
        string? logoUrl = null,
        string? createdBy = null)
    {
        var provider = new ServiceProvider
        {
            Name = name,
            Description = description,
            Type = type,
            ContactEmail = contactEmail ?? string.Empty,
            ContactPhone = contactPhone ?? string.Empty,
            Website = website ?? string.Empty,
            Address = address ?? string.Empty,
            LogoUrl = logoUrl,
            IsActive = true,
            CreatedBy = createdBy
        };

        provider.Apply(new ServiceProviderCreatedEvent(provider.Id, name, type));
        return provider;
    }

    public void Update(
        string name,
        string description,
        ProviderType type,
        string? contactEmail,
        string? contactPhone,
        string? website,
        string? address,
        string? logoUrl,
        string? updatedBy = null)
    {
        Name = name;
        Description = description;
        Type = type;
        ContactEmail = contactEmail ?? string.Empty;
        ContactPhone = contactPhone ?? string.Empty;
        Website = website ?? string.Empty;
        Address = address ?? string.Empty;
        LogoUrl = logoUrl;
        SetUpdated(updatedBy);

        Apply(new ServiceProviderUpdatedEvent(Id, name, type, Version + 1));
    }

    public void Activate(string? updatedBy = null)
    {
        if (IsActive) return;
        IsActive = true;
        SetUpdated(updatedBy);
        Apply(new ServiceProviderActivatedEvent(Id));
    }

    public void Deactivate(string? updatedBy = null)
    {
        if (!IsActive) return;
        IsActive = false;
        SetUpdated(updatedBy);
        Apply(new ServiceProviderDeactivatedEvent(Id));
    }

    protected override void When(DomainEvent @event)
    {
        switch (@event)
        {
            case ServiceProviderCreatedEvent e:
                Id = e.AggregateId;
                Name = e.Name;
                Type = e.Type;
                break;
            case ServiceProviderUpdatedEvent e:
                Name = e.Name;
                Type = e.Type;
                break;
            case ServiceProviderActivatedEvent:
                IsActive = true;
                break;
            case ServiceProviderDeactivatedEvent:
                IsActive = false;
                break;
        }
    }
}
