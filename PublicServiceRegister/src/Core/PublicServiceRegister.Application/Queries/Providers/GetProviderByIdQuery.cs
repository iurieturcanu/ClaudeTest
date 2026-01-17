using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Application.DTOs;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Queries.Providers;

public record GetProviderByIdQuery(Guid Id) : IQuery<PublicServiceProviderDto>;

public class GetProviderByIdQueryHandler : IRequestHandler<GetProviderByIdQuery, Result<PublicServiceProviderDto>>
{
    private readonly IPublicServiceProviderRepository _repository;

    public GetProviderByIdQueryHandler(IPublicServiceProviderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PublicServiceProviderDto>> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (provider == null)
        {
            return Result<PublicServiceProviderDto>.Failure("Provider not found");
        }

        var dto = new PublicServiceProviderDto(
            provider.Id,
            provider.Name,
            provider.Description,
            provider.Type,
            provider.ContactEmail,
            provider.ContactPhone,
            provider.Website,
            provider.Address,
            provider.LogoUrl,
            provider.IsActive,
            provider.Version,
            provider.CreatedAt,
            provider.UpdatedAt);

        return Result<PublicServiceProviderDto>.Success(dto);
    }
}
