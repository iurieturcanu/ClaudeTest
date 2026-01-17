using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Application.DTOs;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Queries.Providers;

public record GetAllProvidersQuery : IQuery<IEnumerable<ServiceProviderDto>>;

public class GetAllProvidersQueryHandler : IRequestHandler<GetAllProvidersQuery, Result<IEnumerable<ServiceProviderDto>>>
{
    private readonly IServiceProviderRepository _repository;

    public GetAllProvidersQueryHandler(IServiceProviderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ServiceProviderDto>>> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
    {
        var providers = await _repository.GetAllAsync(cancellationToken);
        var dtos = providers.Select(p => new ServiceProviderDto(
            p.Id,
            p.Name,
            p.Description,
            p.ProviderType,
            p.RegistrationNumber,
            p.ContactEmail,
            p.ContactPhone,
            p.Address,
            p.Website,
            p.IsActive,
            p.Version,
            p.CreatedAt,
            p.UpdatedAt));

        return Result<IEnumerable<ServiceProviderDto>>.Success(dtos);
    }
}
