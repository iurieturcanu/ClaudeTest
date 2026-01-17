using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicServiceRegister.Application.Commands.Categories;
using PublicServiceRegister.Application.Queries.Categories;

namespace PublicServiceRegister.Api.Controllers;

[Authorize]
public class CategoriesController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
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
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(id, request.Name, request.Description);
        var result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ActivateCategoryCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeactivateCategoryCommand(id), cancellationToken);
        return HandleResult(result);
    }
}

public record UpdateCategoryRequest(string Name, string? Description);
