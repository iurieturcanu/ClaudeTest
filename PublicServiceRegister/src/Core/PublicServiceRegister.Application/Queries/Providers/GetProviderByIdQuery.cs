using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Application.DTOs;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Queries.Providers;

public record GetProviderByIdQuery(Guid Id) : IQuery<ServiceProviderDto>;

public class GetProviderByIdQueryHandler : IRequestHandler<GetProviderByIdQuery, Result<ServiceProviderDto>>
{
    private readonly IServiceProviderRepository _repository;

    public GetProviderByIdQueryHandler(IServiceProviderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ServiceProviderDto>> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (provider == null)
        {
            return Result<ServiceProviderDto>.Failure("Provider not found");
        }

        var dto = new ServiceProviderDto(
            provider.Id,
            provider.Name,
            provider.Description,
            provider.ProviderType,
            provider.RegistrationNumber,
            provider.ContactEmail,
            provider.ContactPhone,
            provider.Address,
            provider.Website,
            provider.IsActive,
            provider.Version,
            provider.CreatedAt,
            provider.UpdatedAt);

        return Result<ServiceProviderDto>.Success(dto);
    }
}
