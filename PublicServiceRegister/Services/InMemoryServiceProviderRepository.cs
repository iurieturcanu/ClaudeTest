using PublicServiceRegister.Models;

namespace PublicServiceRegister.Services;

public class InMemoryServiceProviderRepository : IServiceProviderRepository
{
    private readonly List<ServiceProvider> _providers;
    private readonly IVersionRepository _versionRepository;
    private int _nextId;

    public InMemoryServiceProviderRepository(IVersionRepository versionRepository)
    {
        _versionRepository = versionRepository;
        _providers = new List<ServiceProvider>
        {
            new()
            {
                Id = 1,
                Name = "City Health Department",
                Description = "Municipal department responsible for public health services and disease prevention programs.",
                Type = ProviderType.Municipal,
                ContactEmail = "health@city.gov",
                ContactPhone = "+1-555-0100",
                Website = "https://health.city.gov",
                Address = "100 Health Plaza, Downtown",
                IsActive = true,
                ServiceIds = new List<int> { 1 }
            },
            new()
            {
                Id = 2,
                Name = "City Education Department",
                Description = "Oversees public education facilities, programs, and resources for the community.",
                Type = ProviderType.Municipal,
                ContactEmail = "education@city.gov",
                ContactPhone = "+1-555-0200",
                Website = "https://education.city.gov",
                Address = "200 Education Center, Central District",
                IsActive = true,
                ServiceIds = new List<int> { 2 }
            },
            new()
            {
                Id = 3,
                Name = "Metro Transit Authority",
                Description = "Regional authority managing public transportation including buses, trains, and transit infrastructure.",
                Type = ProviderType.StateAgency,
                ContactEmail = "info@metrotransit.gov",
                ContactPhone = "+1-555-0300",
                Website = "https://metrotransit.gov",
                Address = "300 Transit Hub, Business District",
                IsActive = true,
                ServiceIds = new List<int> { 3 }
            },
            new()
            {
                Id = 4,
                Name = "Department of Social Services",
                Description = "State agency providing welfare, family assistance, and social support programs.",
                Type = ProviderType.StateAgency,
                ContactEmail = "help@socialservices.gov",
                ContactPhone = "+1-555-0400",
                Website = "https://socialservices.gov",
                Address = "400 Community Center, West Side",
                IsActive = true,
                ServiceIds = new List<int> { 4 }
            },
            new()
            {
                Id = 5,
                Name = "City Justice Department",
                Description = "Provides legal services, court administration, and access to justice programs.",
                Type = ProviderType.Municipal,
                ContactEmail = "justice@city.gov",
                ContactPhone = "+1-555-0500",
                Website = "https://justice.city.gov",
                Address = "500 Justice Plaza, Court District",
                IsActive = true,
                ServiceIds = new List<int> { 5 }
            },
            new()
            {
                Id = 6,
                Name = "City Housing Department",
                Description = "Manages public housing, rental assistance, and housing development programs.",
                Type = ProviderType.Municipal,
                ContactEmail = "housing@city.gov",
                ContactPhone = "+1-555-0600",
                Website = "https://housing.city.gov",
                Address = "600 Housing Authority Building, North District",
                IsActive = true,
                ServiceIds = new List<int> { 6 }
            },
            new()
            {
                Id = 7,
                Name = "Department of Labor",
                Description = "Federal agency overseeing employment services, workforce development, and labor standards.",
                Type = ProviderType.FederalAgency,
                ContactEmail = "employment@labor.gov",
                ContactPhone = "+1-555-0700",
                Website = "https://labor.gov",
                Address = "700 Employment Center, Industrial Park",
                IsActive = true,
                ServiceIds = new List<int> { 7 }
            },
            new()
            {
                Id = 8,
                Name = "City Utilities Commission",
                Description = "Manages water, sewage, and other essential utility services for the municipality.",
                Type = ProviderType.Municipal,
                ContactEmail = "utilities@city.gov",
                ContactPhone = "+1-555-0800",
                Website = "https://utilities.city.gov",
                Address = "800 Utility Road, East District",
                IsActive = true,
                ServiceIds = new List<int> { 8 }
            },
            new()
            {
                Id = 9,
                Name = "Community Health Partners",
                Description = "Non-profit organization providing community health outreach and wellness programs.",
                Type = ProviderType.NonProfit,
                ContactEmail = "info@chpartners.org",
                ContactPhone = "+1-555-0900",
                Website = "https://communityhealthpartners.org",
                Address = "900 Wellness Way, South District",
                IsActive = true,
                ServiceIds = new List<int>()
            },
            new()
            {
                Id = 10,
                Name = "TechServe Solutions",
                Description = "Private contractor providing IT and digital services for government agencies.",
                Type = ProviderType.PrivateContractor,
                ContactEmail = "gov@techserve.com",
                ContactPhone = "+1-555-1000",
                Website = "https://techserve.com/government",
                Address = "1000 Tech Park, Innovation District",
                IsActive = true,
                ServiceIds = new List<int>()
            }
        };
        _nextId = _providers.Max(p => p.Id) + 1;
    }

    public Task<IEnumerable<ServiceProvider>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<ServiceProvider>>(_providers.ToList());
    }

    public Task<ServiceProvider?> GetByIdAsync(int id)
    {
        return Task.FromResult(_providers.FirstOrDefault(p => p.Id == id));
    }

    public Task<IEnumerable<ServiceProvider>> GetByTypeAsync(ProviderType type)
    {
        var providers = _providers.Where(p => p.Type == type).ToList();
        return Task.FromResult<IEnumerable<ServiceProvider>>(providers);
    }

    public Task<IEnumerable<ServiceProvider>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        var providers = _providers.Where(p =>
            p.Name.ToLower().Contains(term) ||
            p.Description.ToLower().Contains(term)).ToList();

        return Task.FromResult<IEnumerable<ServiceProvider>>(providers);
    }

    public async Task<ServiceProvider> AddAsync(ServiceProvider provider)
    {
        provider.Id = _nextId++;
        provider.CreatedAt = DateTime.UtcNow;
        provider.Version = 1;
        _providers.Add(provider);

        // Save initial version
        await _versionRepository.SaveVersionAsync(provider, provider.Id, provider.Version, provider.ModifiedBy, "Initial creation");

        return provider;
    }

    public async Task<ServiceProvider> UpdateAsync(ServiceProvider provider, string? modifiedBy = null, string? changeDescription = null)
    {
        var existing = _providers.FirstOrDefault(p => p.Id == provider.Id);
        if (existing != null)
        {
            // Increment version
            existing.Version++;
            existing.Name = provider.Name;
            existing.Description = provider.Description;
            existing.Type = provider.Type;
            existing.ContactEmail = provider.ContactEmail;
            existing.ContactPhone = provider.ContactPhone;
            existing.Website = provider.Website;
            existing.Address = provider.Address;
            existing.LogoUrl = provider.LogoUrl;
            existing.IsActive = provider.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.ModifiedBy = modifiedBy;

            // Save version history
            await _versionRepository.SaveVersionAsync(existing, existing.Id, existing.Version, modifiedBy, changeDescription ?? "Updated");

            return existing;
        }
        return provider;
    }

    public Task<bool> DeleteAsync(int id)
    {
        var provider = _providers.FirstOrDefault(p => p.Id == id);
        if (provider != null)
        {
            _providers.Remove(provider);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<IEnumerable<ServiceProvider>> GetActiveProvidersAsync()
    {
        var providers = _providers.Where(p => p.IsActive).ToList();
        return Task.FromResult<IEnumerable<ServiceProvider>>(providers);
    }
}
