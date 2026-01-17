namespace PublicServiceRegister.Contracts;

public record PublicServiceProviderDto(
    Guid Id,
    string Name,
    string? Description,
    string Type,
    string? ContactEmail,
    string? ContactPhone,
    string? Website,
    string? Address,
    string? LogoUrl,
    bool IsActive,
    int Version,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record CreateProviderRequest(
    string Name,
    string? Description,
    string Type,
    string? ContactEmail,
    string? ContactPhone,
    string? Website,
    string? Address,
    string? LogoUrl);

public record UpdateProviderRequest(
    string Name,
    string? Description,
    string Type,
    string? ContactEmail,
    string? ContactPhone,
    string? Website,
    string? Address,
    string? LogoUrl);
