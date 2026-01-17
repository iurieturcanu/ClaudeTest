using PublicServiceRegister.Domain.Enums;

namespace PublicServiceRegister.Application.DTOs;

public record PublicServiceProviderDto(
    Guid Id,
    string Name,
    string Description,
    ProviderType Type,
    string ContactEmail,
    string ContactPhone,
    string Website,
    string Address,
    string? LogoUrl,
    bool IsActive,
    int Version,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record ServiceProviderListDto(
    Guid Id,
    string Name,
    string Description,
    ProviderType Type,
    string ContactEmail,
    bool IsActive,
    int ServiceCount
);
