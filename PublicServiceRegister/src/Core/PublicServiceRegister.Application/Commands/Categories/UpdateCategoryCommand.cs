using FluentValidation;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Categories;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string Description,
    string? IconClass,
    string? UpdatedBy
) : ICommand;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IServiceCategoryRepository _repository;
    private readonly IEventStore _eventStore;

    public UpdateCategoryCommandHandler(IServiceCategoryRepository repository, IEventStore eventStore)
    {
        _repository = repository;
        _eventStore = eventStore;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failure("Category not found");
        }

        category.Update(request.Name, request.Description, request.IconClass, request.UpdatedBy);

        await _repository.UpdateAsync(category, cancellationToken);
        await _eventStore.SaveEventsAsync(category.Id, category.DomainEvents, category.Version - 1, cancellationToken);

        return Result.Success();
    }
}
