using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Application.DTOs;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Queries.Categories;

public record GetAllCategoriesQuery(bool ActiveOnly = false) : IQuery<IEnumerable<ServiceCategoryDto>>;

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<ServiceCategoryDto>>
{
    private readonly IServiceCategoryRepository _repository;

    public GetAllCategoriesQueryHandler(IServiceCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ServiceCategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = request.ActiveOnly
            ? await _repository.GetActiveAsync(cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        var dtos = categories.Select(c => new ServiceCategoryDto(
            c.Id,
            c.Name,
            c.Description,
            c.IconClass,
            c.IsActive,
            c.Version,
            c.CreatedAt,
            c.UpdatedAt));

        return Result.Success(dtos);
    }
}
