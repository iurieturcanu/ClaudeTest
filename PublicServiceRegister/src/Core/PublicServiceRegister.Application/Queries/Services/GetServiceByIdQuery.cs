using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Application.DTOs;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Queries.Services;

public record GetServiceByIdQuery(Guid Id) : IQuery<PublicServiceDto>;

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, Result<PublicServiceDto>>
{
    private readonly IPublicServiceRepository _repository;

    public GetServiceByIdQueryHandler(IPublicServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PublicServiceDto>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            return Result<PublicServiceDto>.Failure("Service not found");
        }

        var dto = new PublicServiceDto(
            service.Id,
            service.Name,
            service.Description,
            service.CategoryId,
            service.Category?.Name,
            service.ProviderId,
            service.Provider?.Name,
            service.Status,
            service.Requirements,
            service.Fees,
            service.ProcessingTime,
            service.ContactInfo,
            service.OnlineServiceUrl,
            service.Version,
            service.CreatedAt,
            service.UpdatedAt,
            service.PublishedAt);

        return Result<PublicServiceDto>.Success(dto);
    }
}
