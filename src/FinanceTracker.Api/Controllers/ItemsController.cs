using FinanceTracker.Application.Items.Commands.CreateItem;
using FinanceTracker.Application.Items.DTOs;

using MediatR;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/items")]
public class ItemsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create(
        CreateItemCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var item = await _mediator.Send(cmd, cancellationToken);
        return Ok(item);
    }
}
