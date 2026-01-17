using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicServiceRegister.Application.Commands.Services;
using PublicServiceRegister.Application.Queries.Services;
using PublicServiceRegister.Domain.Enums;

namespace PublicServiceRegister.Api.Controllers;

[Authorize]
public class PublicServicesController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllServicesQuery(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetActiveServicesQuery(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetServiceByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("category/{categoryId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategory(Guid categoryId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetServicesByCategoryQuery(categoryId), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("provider/{providerId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByProvider(Guid providerId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetServicesByProviderQuery(providerId), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> Search([FromQuery] string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SearchServicesQuery(term), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Create([FromBody] CreateServiceCommand command, CancellationToken cancellationToken)
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
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateServiceRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateServiceCommand(
            id,
            request.Name,
            request.Description,
            request.CategoryId,
            request.ProviderId,
            request.Requirements,
            request.Fees,
            request.ProcessingTime,
            request.ContactInfo,
            request.OnlineServiceUrl);
        var result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteServiceCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/publish")]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Publish(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new PublishServiceCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/archive")]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Archive(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ArchiveServiceCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/submit-for-review")]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> SubmitForReview(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SubmitServiceForReviewCommand(id), cancellationToken);
        return HandleResult(result);
    }
}

public record UpdateServiceRequest(
    string Name,
    string? Description,
    Guid CategoryId,
    Guid ProviderId,
    string? Requirements,
    string? Fees,
    string? ProcessingTime,
    string? ContactInfo,
    string? OnlineServiceUrl);
