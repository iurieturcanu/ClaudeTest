namespace PublicServiceRegister.Contracts;

public record PublicServiceProviderDto(
    Guid Id,
    string Name,
    string? Description,
    string ProviderType,
    string? RegistrationNumber,
    string? ContactEmail,
    string? ContactPhone,
    string? Address,
    string? Website,
    bool IsActive,
    int Version,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record CreateProviderRequest(
    string Name,
    string? Description,
    string ProviderType,
    string? RegistrationNumber,
    string? ContactEmail,
    string? ContactPhone,
    string? Address,
    string? Website);

public record UpdateProviderRequest(
    string Name,
    string? Description,
    string ProviderType,
    string? RegistrationNumber,
    string? ContactEmail,
    string? ContactPhone,
    string? Address,
    string? Website);
