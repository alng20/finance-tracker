using FinanceTracker.Application.ItemCategories.Commands.CreateItemCategory;
using FinanceTracker.Application.ItemCategories.DTOs;
using FinanceTracker.Application.ItemCategories.Queries.GetItemCategories;

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

    // TODO: [Authorize(Roles="Admin")]
    [HttpPost]
    public async Task<ActionResult<ItemCategoryResultDto>> Create(
        [FromBody] CreateItemCategoryCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var category = await _mediator.Send(cmd, cancellationToken);
        return Ok(category);
    }
}
