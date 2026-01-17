using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Providers;

public record ActivateProviderCommand(Guid Id) : ICommand;

public class ActivateProviderCommandHandler : IRequestHandler<ActivateProviderCommand, Result>
{
    private readonly IServiceProviderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateProviderCommandHandler(IServiceProviderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ActivateProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (provider == null)
        {
            return Result.Failure("Provider not found");
        }

        provider.Activate();
        await _repository.UpdateAsync(provider, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
