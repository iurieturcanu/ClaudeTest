using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Services;

public record PublishServiceCommand(Guid Id) : ICommand;

public class PublishServiceCommandHandler : IRequestHandler<PublishServiceCommand, Result>
{
    private readonly IPublicServiceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishServiceCommandHandler(IPublicServiceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PublishServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            return Result.Failure("Service not found");
        }

        service.Publish();
        await _repository.UpdateAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
