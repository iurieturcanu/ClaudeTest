using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Services;

public record DeleteServiceCommand(Guid Id) : ICommand;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, Result>
{
    private readonly IPublicServiceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceCommandHandler(IPublicServiceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            return Result.Failure("Service not found");
        }

        await _repository.DeleteAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
