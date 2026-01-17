using FluentValidation;
using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Enums;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Providers;

public record UpdateProviderCommand(
    Guid Id,
    string Name,
    string? Description,
    ProviderType ProviderType,
    string? RegistrationNumber,
    string? ContactEmail,
    string? ContactPhone,
    string? Address,
    string? Website) : ICommand;

public class UpdateProviderCommandValidator : AbstractValidator<UpdateProviderCommand>
{
    public UpdateProviderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(300).WithMessage("Name must not exceed 300 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.ContactEmail)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.ContactEmail))
            .WithMessage("Invalid email format");
    }
}

public class UpdateProviderCommandHandler : IRequestHandler<UpdateProviderCommand, Result>
{
    private readonly IServiceProviderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProviderCommandHandler(IServiceProviderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (provider == null)
        {
            return Result.Failure("Provider not found");
        }

        provider.Update(
            request.Name,
            request.Description,
            request.ProviderType,
            request.RegistrationNumber,
            request.ContactEmail,
            request.ContactPhone,
            request.Address,
            request.Website);

        await _repository.UpdateAsync(provider, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
