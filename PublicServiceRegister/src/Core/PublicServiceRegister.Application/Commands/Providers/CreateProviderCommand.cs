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
    ProviderType Type,
    string? ContactEmail,
    string? ContactPhone,
    string? Website,
    string? Address,
    string? LogoUrl) : ICommand<Guid>;

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
        var provider = PublicServiceProvider.Create(
            request.Name,
            request.Description ?? string.Empty,
            request.Type,
            request.ContactEmail,
            request.ContactPhone,
            request.Website,
            request.Address,
            request.LogoUrl);

        await _repository.AddAsync(provider, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(provider.Id);
    }
}
