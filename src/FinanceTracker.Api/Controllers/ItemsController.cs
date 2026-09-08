using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Items.Commands.CreateItem;
using FinanceTracker.Application.Items.Commands.DeleteItem;
using FinanceTracker.Application.Items.Commands.UpdateItem;
using FinanceTracker.Application.Items.DTOs;
using FinanceTracker.Application.Items.Queries.GetItemById;
using FinanceTracker.Application.Items.Queries.GetItems;
using FinanceTracker.Application.Items.Queries.SearchItem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/items")]
public class ItemsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var item = await _mediator.Send(new GetItemByIdQuery(id), cancellationToken);
        return Ok(item);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<GetItemsResultDto>>> Get(
        [FromQuery] GetItemsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var item = await _mediator.Send(query, cancellationToken);
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create(
        CreateItemCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var item = await _mediator.Send(cmd, cancellationToken);
        return Ok(item);
    }

    // [Authorize(Roles="Admin")]
    [HttpPut]
    public async Task<ActionResult<ItemDto>> Update(
        [FromBody] UpdateItemCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var item = await _mediator.Send(cmd, cancellationToken);
        return Ok(item);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<SearchItemResultDto>>> Search(
        [FromQuery] string searchString,
        CancellationToken cancellationToken
    )
    {
        var items = await _mediator.Send(new SearchItemQuery(searchString), cancellationToken);
        return Ok(items);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteItemCommand(id), cancellationToken);
        return NoContent();
    }
}
