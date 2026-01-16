using PublicServiceRegister.Models;

namespace PublicServiceRegister.Services;

public class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly List<ServiceCategory> _categories;
    private int _nextId;

    public InMemoryCategoryRepository()
    {
        _categories = new List<ServiceCategory>
        {
            new() { Id = 1, Name = "Healthcare", Description = "Medical and health-related services", IconClass = "bi bi-hospital" },
            new() { Id = 2, Name = "Education", Description = "Educational services and institutions", IconClass = "bi bi-book" },
            new() { Id = 3, Name = "Transportation", Description = "Public transportation services", IconClass = "bi bi-bus-front" },
            new() { Id = 4, Name = "Social Services", Description = "Social welfare and support services", IconClass = "bi bi-people" },
            new() { Id = 5, Name = "Legal", Description = "Legal and judicial services", IconClass = "bi bi-briefcase" },
            new() { Id = 6, Name = "Housing", Description = "Housing and accommodation services", IconClass = "bi bi-house" },
            new() { Id = 7, Name = "Employment", Description = "Job and employment services", IconClass = "bi bi-person-badge" },
            new() { Id = 8, Name = "Utilities", Description = "Public utility services", IconClass = "bi bi-lightning" }
        };
        _nextId = _categories.Max(c => c.Id) + 1;
    }

    public Task<IEnumerable<ServiceCategory>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<ServiceCategory>>(_categories.ToList());
    }

    public Task<ServiceCategory?> GetByIdAsync(int id)
    {
        return Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));
    }

    public Task<ServiceCategory> AddAsync(ServiceCategory category)
    {
        category.Id = _nextId++;
        _categories.Add(category);
        return Task.FromResult(category);
    }

    public Task<ServiceCategory> UpdateAsync(ServiceCategory category)
    {
        var existing = _categories.FirstOrDefault(c => c.Id == category.Id);
        if (existing != null)
        {
            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.IconClass = category.IconClass;
        }
        return Task.FromResult(existing ?? category);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category != null)
        {
            _categories.Remove(category);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
