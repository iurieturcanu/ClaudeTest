using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Application.DTOs;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Queries.Providers;

public record GetAllProvidersQuery : IQuery<IEnumerable<PublicServiceProviderDto>>;

public class GetAllProvidersQueryHandler : IRequestHandler<GetAllProvidersQuery, Result<IEnumerable<PublicServiceProviderDto>>>
{
    private readonly IPublicServiceProviderRepository _repository;

    public GetAllProvidersQueryHandler(IPublicServiceProviderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<PublicServiceProviderDto>>> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
    {
        var providers = await _repository.GetAllAsync(cancellationToken);
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
