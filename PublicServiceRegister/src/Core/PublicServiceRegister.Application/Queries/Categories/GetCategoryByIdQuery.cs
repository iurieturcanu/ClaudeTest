using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Application.DTOs;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Queries.Categories;

public record GetCategoryByIdQuery(Guid Id) : IQuery<ServiceCategoryDto>;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, ServiceCategoryDto>
{
    private readonly IServiceCategoryRepository _repository;

    public GetCategoryByIdQueryHandler(IServiceCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ServiceCategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failure<ServiceCategoryDto>("Category not found");
        }

        return new ServiceCategoryDto(
            category.Id,
            category.Name,
            category.Description,
            category.IconClass,
            category.IsActive,
            category.Version,
            category.CreatedAt,
            category.UpdatedAt);
    }
}
