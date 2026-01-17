namespace PublicServiceRegister.Contracts;

public record PublicServiceDto(
    Guid Id,
    string Name,
    string? Description,
    Guid CategoryId,
    string? CategoryName,
    Guid ProviderId,
    string? ProviderName,
    string Status,
    string? Requirements,
    string? Fees,
    string? ProcessingTime,
    string? ContactInfo,
    string? OnlineServiceUrl,
    int Version,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? PublishedAt);

public record CreateServiceRequest(
    string Name,
    string? Description,
    Guid CategoryId,
    Guid ProviderId,
    string? Requirements,
    string? Fees,
    string? ProcessingTime,
    string? ContactInfo,
    string? OnlineServiceUrl);

public record UpdateServiceRequest(
    string Name,
    string? Description,
    Guid CategoryId,
    Guid ProviderId,
    string? Requirements,
    string? Fees,
    string? ProcessingTime,
    string? ContactInfo,
    string? OnlineServiceUrl);
