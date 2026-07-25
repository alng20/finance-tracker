using FinanceTracker.Application.ItemCategories.Commands.CreateItemCategory;
using FinanceTracker.Application.ItemCategories.Commands.DeleteItemCategory;
using FinanceTracker.Application.ItemCategories.Commands.UpdateItemCategory;
using FinanceTracker.Application.ItemCategories.DTOs;
using FinanceTracker.Application.ItemCategories.Queries.GetItemCategories;
using FinanceTracker.Application.ItemCategories.Queries.GetItemCategory;

using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/categories")]
public class ItemCategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ItemCategoryResultDto>>> GetAll(
        CancellationToken cancellationToken
    )
    {
        var categories = await _mediator.Send(new GetItemCategoriesQuery(), cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemCategoryResultDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var category = await _mediator.Send(new GetItemCategoryQuery(id), cancellationToken);
        return Ok(category);
    }

    // TODO: [Authorize(Roles="Admin")]
    [HttpPost]
    public async Task<ActionResult<ItemCategoryResultDto>> Create(
        [FromBody] CreateItemCategoryCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var category = await _mediator.Send(cmd, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    // TODO: [Authorize(Roles="Admin")]
    [HttpPut]
    public async Task<ActionResult<ItemCategoryResultDto>> Update(
        [FromBody] UpdateItemCategoryCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var category = await _mediator.Send(cmd, cancellationToken);
        return Ok(category);
    }

    // TODO: [Authorize(Roles="Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new DeleteItemCategoryCommand(id), cancellationToken);
        return NoContent();
    }
}
