using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Application.DTOs;
using PublicServiceRegister.Domain.Enums;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Queries.Providers;

public record GetProvidersByTypeQuery(ProviderType ProviderType) : IQuery<IEnumerable<PublicServiceProviderDto>>;

public class GetProvidersByTypeQueryHandler : IRequestHandler<GetProvidersByTypeQuery, Result<IEnumerable<PublicServiceProviderDto>>>
{
    private readonly IPublicServiceProviderRepository _repository;

    public GetProvidersByTypeQueryHandler(IPublicServiceProviderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<PublicServiceProviderDto>>> Handle(GetProvidersByTypeQuery request, CancellationToken cancellationToken)
    {
        var providers = await _repository.GetByTypeAsync(request.ProviderType, cancellationToken);
        var dtos = providers.Select(p => new PublicServiceProviderDto(
            p.Id,
            p.Name,
            p.Description,
            p.Type,
            p.ContactEmail,
            p.ContactPhone,
            p.Website,
            p.Address,
            p.LogoUrl,
            p.IsActive,
            p.Version,
            p.CreatedAt,
            p.UpdatedAt));

        return Result<IEnumerable<PublicServiceProviderDto>>.Success(dtos);
    }
}
