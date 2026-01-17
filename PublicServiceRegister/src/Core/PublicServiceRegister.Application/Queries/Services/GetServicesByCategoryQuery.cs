using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Application.DTOs;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Queries.Services;

public record GetServicesByCategoryQuery(Guid CategoryId) : IQuery<IEnumerable<PublicServiceDto>>;

public class GetServicesByCategoryQueryHandler : IRequestHandler<GetServicesByCategoryQuery, Result<IEnumerable<PublicServiceDto>>>
{
    private readonly IPublicServiceRepository _repository;

    public GetServicesByCategoryQueryHandler(IPublicServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<PublicServiceDto>>> Handle(GetServicesByCategoryQuery request, CancellationToken cancellationToken)
    {
        var services = await _repository.GetByCategoryAsync(request.CategoryId, cancellationToken);
        var dtos = services.Select(s => new PublicServiceDto(
            s.Id,
            s.Name,
            s.Description,
            s.CategoryId,
            s.Category?.Name,
            s.ProviderId,
            s.Provider?.Name,
            s.Status,
            s.Requirements,
            s.Fees,
            s.ProcessingTime,
            s.ContactInfo,
            s.OnlineServiceUrl,
            s.Version,
            s.CreatedAt,
            s.UpdatedAt,
            s.PublishedAt));

        return Result<IEnumerable<PublicServiceDto>>.Success(dtos);
    }
}
