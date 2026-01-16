using PublicServiceRegister.Models;
using PublicServiceRegister.Models.Versioning;

namespace PublicServiceRegister.Services;

public class InMemoryPublicServiceRepository : IPublicServiceRepository
{
    private readonly List<PublicService> _services;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IVersionRepository _versionRepository;
    private int _nextId;

    public InMemoryPublicServiceRepository(ICategoryRepository categoryRepository, IVersionRepository versionRepository)
    {
        _categoryRepository = categoryRepository;
        _versionRepository = versionRepository;
        _services = new List<PublicService>
        {
            new()
            {
                Id = 1,
                Name = "City General Hospital",
                Description = "Full-service public hospital providing emergency care, outpatient services, and specialized treatments.",
                CategoryId = 1,
                ServiceProviderId = 1,
                Provider = "City Health Department",
                ContactEmail = "info@cityhospital.gov",
                ContactPhone = "+1-555-0100",
                Website = "https://cityhospital.gov",
                Address = "123 Health Avenue, Downtown",
                Status = ServiceStatus.Active,
                OperatingHours = "24/7 Emergency, 8AM-6PM Outpatient",
                RequiredDocuments = new List<string> { "ID Card", "Insurance Card", "Referral Letter (if applicable)" },
                Fee = 0,
                FeeDescription = "Free for emergency services, fees may apply for specialized treatments",
                Version = 1
            },
            new()
            {
                Id = 2,
                Name = "Public Library System",
                Description = "Network of public libraries offering book lending, digital resources, and community programs.",
                CategoryId = 2,
                ServiceProviderId = 2,
                Provider = "City Education Department",
                ContactEmail = "library@city.gov",
                ContactPhone = "+1-555-0200",
                Website = "https://publiclibrary.gov",
                Address = "456 Knowledge Street, Central District",
                Status = ServiceStatus.Active,
                OperatingHours = "Mon-Sat: 9AM-8PM, Sun: 12PM-5PM",
                RequiredDocuments = new List<string> { "ID Card", "Proof of Residence" },
                Fee = 0,
                FeeDescription = "Free library card for residents",
                Version = 1
            },
            new()
            {
                Id = 3,
                Name = "Metro Bus Service",
                Description = "City-wide public bus transportation connecting all major districts and neighborhoods.",
                CategoryId = 3,
                ServiceProviderId = 3,
                Provider = "Metro Transit Authority",
                ContactEmail = "support@metrobus.gov",
                ContactPhone = "+1-555-0300",
                Website = "https://metrobus.gov",
                Address = "789 Transit Center, Business District",
                Status = ServiceStatus.Active,
                OperatingHours = "5AM-12AM Daily",
                RequiredDocuments = new List<string> { "None required" },
                Fee = 2.50m,
                FeeDescription = "Standard fare, discounts available for seniors and students",
                Version = 1
            },
            new()
            {
                Id = 4,
                Name = "Social Welfare Office",
                Description = "Provides assistance programs including food stamps, housing assistance, and family support services.",
                CategoryId = 4,
                ServiceProviderId = 4,
                Provider = "Department of Social Services",
                ContactEmail = "welfare@social.gov",
                ContactPhone = "+1-555-0400",
                Website = "https://socialservices.gov",
                Address = "321 Community Lane, West Side",
                Status = ServiceStatus.Active,
                OperatingHours = "Mon-Fri: 8AM-5PM",
                RequiredDocuments = new List<string> { "ID Card", "Proof of Income", "Proof of Residence", "Bank Statements" },
                Fee = 0,
                FeeDescription = "Free services",
                Version = 1
            },
            new()
            {
                Id = 5,
                Name = "Legal Aid Center",
                Description = "Free legal assistance for low-income residents in civil matters.",
                CategoryId = 5,
                ServiceProviderId = 5,
                Provider = "City Justice Department",
                ContactEmail = "legalaid@justice.gov",
                ContactPhone = "+1-555-0500",
                Website = "https://legalaid.gov",
                Address = "654 Justice Plaza, Court District",
                Status = ServiceStatus.Active,
                OperatingHours = "Mon-Fri: 9AM-4PM",
                RequiredDocuments = new List<string> { "ID Card", "Proof of Income", "Case Documents" },
                Fee = 0,
                FeeDescription = "Free for qualifying residents",
                Version = 1
            },
            new()
            {
                Id = 6,
                Name = "Housing Authority",
                Description = "Manages public housing programs and rental assistance for eligible families.",
                CategoryId = 6,
                ServiceProviderId = 6,
                Provider = "City Housing Department",
                ContactEmail = "housing@city.gov",
                ContactPhone = "+1-555-0600",
                Website = "https://housing.gov",
                Address = "987 Shelter Street, North District",
                Status = ServiceStatus.Active,
                OperatingHours = "Mon-Fri: 8:30AM-4:30PM",
                RequiredDocuments = new List<string> { "ID Card", "Proof of Income", "Family Composition Form", "Background Check Consent" },
                Fee = 0,
                FeeDescription = "Application is free, rent based on income",
                Version = 1
            },
            new()
            {
                Id = 7,
                Name = "Job Training Center",
                Description = "Vocational training and job placement services for unemployed residents.",
                CategoryId = 7,
                ServiceProviderId = 7,
                Provider = "Department of Labor",
                ContactEmail = "jobs@labor.gov",
                ContactPhone = "+1-555-0700",
                Website = "https://jobcenter.gov",
                Address = "159 Career Boulevard, Industrial Park",
                Status = ServiceStatus.Active,
                OperatingHours = "Mon-Fri: 8AM-6PM, Sat: 9AM-1PM",
                RequiredDocuments = new List<string> { "ID Card", "Resume", "Education Certificates" },
                Fee = 0,
                FeeDescription = "Free training programs",
                Version = 1
            },
            new()
            {
                Id = 8,
                Name = "Water & Sewage Department",
                Description = "Manages municipal water supply and sewage treatment services.",
                CategoryId = 8,
                ServiceProviderId = 8,
                Provider = "City Utilities Commission",
                ContactEmail = "water@utilities.gov",
                ContactPhone = "+1-555-0800",
                Website = "https://waterutility.gov",
                Address = "753 Utility Road, East District",
                Status = ServiceStatus.Active,
                OperatingHours = "Mon-Fri: 7AM-5PM, Emergency: 24/7",
                RequiredDocuments = new List<string> { "Property Documents", "ID Card" },
                Fee = null,
                FeeDescription = "Monthly rates based on usage",
                Version = 1
            }
        };
        _nextId = _services.Max(s => s.Id) + 1;
    }

    public async Task<IEnumerable<PublicService>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var categoryDict = categories.ToDictionary(c => c.Id);

        foreach (var service in _services)
        {
            if (categoryDict.TryGetValue(service.CategoryId, out var category))
            {
                service.Category = category;
            }
        }

        return _services.ToList();
    }

    public async Task<PublicService?> GetByIdAsync(int id)
    {
        var service = _services.FirstOrDefault(s => s.Id == id);
        if (service != null)
        {
            service.Category = await _categoryRepository.GetByIdAsync(service.CategoryId);
        }
        return service;
    }

    public async Task<IEnumerable<PublicService>> GetByCategoryAsync(int categoryId)
    {
        var services = _services.Where(s => s.CategoryId == categoryId).ToList();
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        foreach (var service in services)
        {
            service.Category = category;
        }

        return services;
    }

    public Task<IEnumerable<PublicService>> GetByProviderAsync(int providerId)
    {
        var services = _services.Where(s => s.ServiceProviderId == providerId).ToList();
        return Task.FromResult<IEnumerable<PublicService>>(services);
    }

    public async Task<IEnumerable<PublicService>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        var services = _services.Where(s =>
            s.Name.ToLower().Contains(term) ||
            s.Description.ToLower().Contains(term) ||
            s.Provider.ToLower().Contains(term)).ToList();

        var categories = await _categoryRepository.GetAllAsync();
        var categoryDict = categories.ToDictionary(c => c.Id);

        foreach (var service in services)
        {
            if (categoryDict.TryGetValue(service.CategoryId, out var category))
            {
                service.Category = category;
            }
        }

        return services;
    }

    public async Task<PublicService> AddAsync(PublicService service)
    {
        service.Id = _nextId++;
        service.CreatedAt = DateTime.UtcNow;
        service.Version = 1;
        service.Category = await _categoryRepository.GetByIdAsync(service.CategoryId);
        _services.Add(service);

        // Save initial version
        await _versionRepository.SaveVersionAsync(service, service.Id, service.Version, service.ModifiedBy, "Initial creation");

        return service;
    }

    public async Task<PublicService> UpdateAsync(PublicService service, string? modifiedBy = null, string? changeDescription = null)
    {
        var existing = _services.FirstOrDefault(s => s.Id == service.Id);
        if (existing != null)
        {
            // Increment version
            existing.Version++;
            existing.Name = service.Name;
            existing.Description = service.Description;
            existing.CategoryId = service.CategoryId;
            existing.Category = await _categoryRepository.GetByIdAsync(service.CategoryId);
            existing.ServiceProviderId = service.ServiceProviderId;
            existing.Provider = service.Provider;
            existing.ContactEmail = service.ContactEmail;
            existing.ContactPhone = service.ContactPhone;
            existing.Website = service.Website;
            existing.Address = service.Address;
            existing.Status = service.Status;
            existing.OperatingHours = service.OperatingHours;
            existing.RequiredDocuments = service.RequiredDocuments;
            existing.Fee = service.Fee;
            existing.FeeDescription = service.FeeDescription;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.ModifiedBy = modifiedBy;

            // Save version history
            await _versionRepository.SaveVersionAsync(existing, existing.Id, existing.Version, modifiedBy, changeDescription ?? "Updated");

            return existing;
        }
        return service;
    }

    public Task<bool> DeleteAsync(int id)
    {
        var service = _services.FirstOrDefault(s => s.Id == id);
        if (service != null)
        {
            _services.Remove(service);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public async Task<IEnumerable<EntityVersion>> GetVersionHistoryAsync(int serviceId)
    {
        return await _versionRepository.GetVersionHistoryAsync(nameof(PublicService), serviceId);
    }

    public async Task<PublicService?> GetVersionAsync(int serviceId, int versionNumber)
    {
        return await _versionRepository.RestoreVersionAsync<PublicService>(nameof(PublicService), serviceId, versionNumber);
    }
}
