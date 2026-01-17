using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Providers;

public record DeleteProviderCommand(Guid Id) : ICommand;

public class DeleteProviderCommandHandler : IRequestHandler<DeleteProviderCommand, Result>
{
    private readonly IPublicServiceProviderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProviderCommandHandler(IPublicServiceProviderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (provider == null)
        {
            return Result.Failure("Provider not found");
        }

        await _repository.DeleteAsync(provider, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
