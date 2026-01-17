namespace PublicServiceRegister.Contracts;

public record ServiceCategoryDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    int Version,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record CreateCategoryRequest(string Name, string? Description);
public record UpdateCategoryRequest(string Name, string? Description);
