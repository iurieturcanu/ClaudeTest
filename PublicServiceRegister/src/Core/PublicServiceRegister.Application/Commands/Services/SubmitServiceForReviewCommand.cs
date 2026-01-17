using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Services;

public record SubmitServiceForReviewCommand(Guid Id) : ICommand;

public class SubmitServiceForReviewCommandHandler : IRequestHandler<SubmitServiceForReviewCommand, Result>
{
    private readonly IPublicServiceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitServiceForReviewCommandHandler(IPublicServiceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SubmitServiceForReviewCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            return Result.Failure("Service not found");
        }

        service.SubmitForReview();
        await _repository.UpdateAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
