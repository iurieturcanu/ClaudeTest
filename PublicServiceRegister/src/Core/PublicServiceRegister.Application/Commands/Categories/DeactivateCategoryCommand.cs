using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Categories;

public record DeactivateCategoryCommand(Guid Id) : ICommand;

public class DeactivateCategoryCommandHandler : IRequestHandler<DeactivateCategoryCommand, Result>
{
    private readonly IServiceCategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateCategoryCommandHandler(IServiceCategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failure("Category not found");
        }

        category.Deactivate();
        await _repository.UpdateAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
