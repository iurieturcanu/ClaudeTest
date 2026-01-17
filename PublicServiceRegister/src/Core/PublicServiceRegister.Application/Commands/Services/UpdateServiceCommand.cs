using FluentValidation;
using MediatR;
using PublicServiceRegister.Application.Common;
using PublicServiceRegister.Domain.Interfaces;

namespace PublicServiceRegister.Application.Commands.Services;

public record UpdateServiceCommand(
    Guid Id,
    string Name,
    string? Description,
    Guid CategoryId,
    Guid ProviderId,
    string? Requirements,
    string? Fees,
    string? ProcessingTime,
    string? ContactInfo,
    string? OnlineServiceUrl) : ICommand;

public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(300).WithMessage("Name must not exceed 300 characters");

        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ProviderId).NotEmpty().WithMessage("Provider is required");
    }
}

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, Result>
{
    private readonly IPublicServiceRepository _serviceRepository;
    private readonly IServiceCategoryRepository _categoryRepository;
    private readonly IServiceProviderRepository _providerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceCommandHandler(
        IPublicServiceRepository serviceRepository,
        IServiceCategoryRepository categoryRepository,
        IServiceProviderRepository providerRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _categoryRepository = categoryRepository;
        _providerRepository = providerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            return Result.Failure("Service not found");
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result.Failure("Category not found");
        }

        var provider = await _providerRepository.GetByIdAsync(request.ProviderId, cancellationToken);
        if (provider == null)
        {
            return Result.Failure("Provider not found");
        }

        service.Update(
            request.Name,
            request.Description,
            request.CategoryId,
            request.ProviderId,
            request.Requirements,
            request.Fees,
            request.ProcessingTime,
            request.ContactInfo,
            request.OnlineServiceUrl);

        await _serviceRepository.UpdateAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
