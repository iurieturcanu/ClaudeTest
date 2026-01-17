using PublicServiceRegister.Domain.Enums;

namespace PublicServiceRegister.Application.DTOs;

public record PublicServiceDto(
    Guid Id,
    string Name,
    string Description,
    Guid CategoryId,
    string? CategoryName,
    Guid? ServiceProviderId,
    string? ServiceProviderName,
    string ContactEmail,
    string ContactPhone,
    string Website,
    string Address,
    ServiceStatus Status,
    string OperatingHours,
    decimal? Fee,
    string FeeDescription,
    IReadOnlyCollection<string> RequiredDocuments,
    int Version,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record PublicServiceListDto(
    Guid Id,
    string Name,
    string Description,
    string CategoryName,
    string? ServiceProviderName,
    ServiceStatus Status,
    string ContactPhone
);
