using FluentValidation;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Entities;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Categories;

public record CreateCategoryCommand(
    string Name,
    string Description,
    string? IconClass,
    string? CreatedBy
) : ICommand<Guid>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
    }
}

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly IServiceCategoryRepository _repository;
    private readonly IEventStore _eventStore;

    public CreateCategoryCommandHandler(IServiceCategoryRepository repository, IEventStore eventStore)
    {
        _repository = repository;
        _eventStore = eventStore;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await _repository.GetByNameAsync(request.Name, cancellationToken);
        if (existingCategory != null)
        {
            return Result.Failure<Guid>("A category with this name already exists");
        }

        var category = ServiceCategory.Create(
            request.Name,
            request.Description,
            request.IconClass,
            request.CreatedBy);

        await _repository.AddAsync(category, cancellationToken);
        await _eventStore.SaveEventsAsync(category.Id, category.DomainEvents, 0, cancellationToken);

        return Result.Success(category.Id);
    }
}
