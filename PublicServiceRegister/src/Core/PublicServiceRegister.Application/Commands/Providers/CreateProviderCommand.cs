using FluentValidation;
using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Entities;
using PublicServiceRegister.Domain.Enums;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Providers;

public record CreateProviderCommand(
    string Name,
    string? Description,
    ProviderType ProviderType,
    string? RegistrationNumber,
    string? ContactEmail,
    string? ContactPhone,
    string? Address,
    string? Website) : ICommand<Guid>;

public class CreateProviderCommandValidator : AbstractValidator<CreateProviderCommand>
{
    public CreateProviderCommandValidator()
    {
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

public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, Result<Guid>>
{
    private readonly IPublicServiceProviderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProviderCommandHandler(IPublicServiceProviderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.RegistrationNumber))
        {
            var existing = await _repository.GetByRegistrationNumberAsync(request.RegistrationNumber, cancellationToken);
            if (existing != null)
            {
                return Result<Guid>.Failure("A provider with this registration number already exists");
            }
        }

        var provider = PublicServiceProvider.Create(
            request.Name,
            request.Description,
            request.ProviderType,
            request.RegistrationNumber,
            request.ContactEmail,
            request.ContactPhone,
            request.Address,
            request.Website);

        await _repository.AddAsync(provider, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(provider.Id);
    }
}
