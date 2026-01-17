using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Providers;

public record DeactivateProviderCommand(Guid Id) : ICommand;

public class DeactivateProviderCommandHandler : IRequestHandler<DeactivateProviderCommand, Result>
{
    private readonly IPublicServiceProviderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateProviderCommandHandler(IPublicServiceProviderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (provider == null)
        {
            return Result.Failure("Provider not found");
        }

        provider.Deactivate();
        await _repository.UpdateAsync(provider, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
