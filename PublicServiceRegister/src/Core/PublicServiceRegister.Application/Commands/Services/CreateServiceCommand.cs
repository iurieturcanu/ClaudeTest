using FluentValidation;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Entities;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Services;

public record CreateServiceCommand(
    string Name,
    string Description,
    Guid CategoryId,
    Guid? ServiceProviderId,
    string? ContactEmail,
    string? ContactPhone,
    string? Website,
    string? Address,
    string? OperatingHours,
    decimal? Fee,
    string? FeeDescription,
    IEnumerable<string>? RequiredDocuments,
    string? CreatedBy
) : ICommand<Guid>;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.ContactEmail).EmailAddress().When(x => !string.IsNullOrEmpty(x.ContactEmail));
    }
}

public class CreateServiceCommandHandler : ICommandHandler<CreateServiceCommand, Guid>
{
    private readonly IPublicServiceRepository _repository;
    private readonly IServiceCategoryRepository _categoryRepository;
    private readonly IEventStore _eventStore;

    public CreateServiceCommandHandler(
        IPublicServiceRepository repository,
        IServiceCategoryRepository categoryRepository,
        IEventStore eventStore)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _eventStore = eventStore;
    }

    public async Task<Result<Guid>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result.Failure<Guid>("Category not found");
        }

        var service = PublicService.Create(
            request.Name,
            request.Description,
            request.CategoryId,
            request.ServiceProviderId,
            request.ContactEmail,
            request.ContactPhone,
            request.Website,
            request.Address,
            request.OperatingHours,
            request.Fee,
            request.FeeDescription,
            request.RequiredDocuments,
            request.CreatedBy);

        await _repository.AddAsync(service, cancellationToken);
        await _eventStore.SaveEventsAsync(service.Id, service.DomainEvents, 0, cancellationToken);

        return Result.Success(service.Id);
    }
}
