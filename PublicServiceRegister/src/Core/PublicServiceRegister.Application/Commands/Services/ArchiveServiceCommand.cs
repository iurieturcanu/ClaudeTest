using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Services;

public record ArchiveServiceCommand(Guid Id) : ICommand;

public class ArchiveServiceCommandHandler : IRequestHandler<ArchiveServiceCommand, Result>
{
    private readonly IPublicServiceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveServiceCommandHandler(IPublicServiceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ArchiveServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            return Result.Failure("Service not found");
        }

        service.Archive();
        await _repository.UpdateAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
