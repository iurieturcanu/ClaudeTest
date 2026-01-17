using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Categories;

public record ActivateCategoryCommand(Guid Id) : ICommand;

public class ActivateCategoryCommandHandler : IRequestHandler<ActivateCategoryCommand, Result>
{
    private readonly IServiceCategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateCategoryCommandHandler(IServiceCategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ActivateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failure("Category not found");
        }

        category.Activate();
        await _repository.UpdateAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
