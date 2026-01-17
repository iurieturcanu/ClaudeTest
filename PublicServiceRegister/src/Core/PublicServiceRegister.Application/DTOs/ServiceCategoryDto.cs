namespace PublicServiceRegister.Application.DTOs;

public record ServiceCategoryDto(
    Guid Id,
    string Name,
    string Description,
    string IconClass,
    bool IsActive,
    int Version,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record ServiceCategoryListDto(
    Guid Id,
    string Name,
    string Description,
    string IconClass,
    bool IsActive,
    int ServiceCount
);
