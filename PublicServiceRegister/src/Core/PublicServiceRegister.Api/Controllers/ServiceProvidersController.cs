using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicServiceRegister.Application.Commands.Providers;
using PublicServiceRegister.Application.Queries.Providers;
using PublicServiceRegister.Domain.Enums;

namespace PublicServiceRegister.Api.Controllers;

[Authorize]
public class PublicServiceProvidersController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllProvidersQuery(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProviderByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("type/{providerType}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByType(ProviderType providerType, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProvidersByTypeQuery(providerType), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Create([FromBody] CreateProviderCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
        }
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProviderRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProviderCommand(
            id,
            request.Name,
            request.Description,
            request.Type,
            request.ContactEmail,
            request.ContactPhone,
            request.Website,
            request.Address,
            request.LogoUrl);
        var result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteProviderCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ActivateProviderCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeactivateProviderCommand(id), cancellationToken);
        return HandleResult(result);
    }
}

public record UpdateProviderRequest(
    string Name,
    string? Description,
    ProviderType Type,
    string? ContactEmail,
    string? ContactPhone,
    string? Website,
    string? Address,
    string? LogoUrl);
