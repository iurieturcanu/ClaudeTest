using PublicServiceRegister.Domain.Common;
using PublicServiceRegister.Domain.Events;

namespace PublicServiceRegister.Domain.Entities;

public class ServiceCategory : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string IconClass { get; private set; } = "bi bi-folder";
    public bool IsActive { get; private set; } = true;

    private ServiceCategory() : base() { }

    public static ServiceCategory Create(string name, string description, string? iconClass = null, string? createdBy = null)
    {
        var category = new ServiceCategory
        {
            Name = name,
            Description = description,
            IconClass = iconClass ?? "bi bi-folder",
            IsActive = true,
            CreatedBy = createdBy
        };

        category.Apply(new ServiceCategoryCreatedEvent(category.Id, name, description));
        return category;
    }

    public void Update(string name, string description, string? iconClass, string? updatedBy = null)
    {
        Name = name;
        Description = description;
        if (iconClass != null) IconClass = iconClass;
        SetUpdated(updatedBy);

        Apply(new ServiceCategoryUpdatedEvent(Id, name, description, Version + 1));
    }

    public void Activate(string? updatedBy = null)
    {
        if (IsActive) return;
        IsActive = true;
        SetUpdated(updatedBy);
        Apply(new ServiceCategoryActivatedEvent(Id));
    }

    public void Deactivate(string? updatedBy = null)
    {
        if (!IsActive) return;
        IsActive = false;
        SetUpdated(updatedBy);
        Apply(new ServiceCategoryDeactivatedEvent(Id));
    }

    protected override void When(DomainEvent @event)
    {
        switch (@event)
        {
            case ServiceCategoryCreatedEvent e:
                Id = e.AggregateId;
                Name = e.Name;
                Description = e.Description;
                break;
            case ServiceCategoryUpdatedEvent e:
                Name = e.Name;
                Description = e.Description;
                break;
            case ServiceCategoryActivatedEvent:
                IsActive = true;
                break;
            case ServiceCategoryDeactivatedEvent:
                IsActive = false;
                break;
        }
    }
}
