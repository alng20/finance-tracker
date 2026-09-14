using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Shops.Commands.CreateShop;
using FinanceTracker.Application.Shops.Commands.DeleteShop;
using FinanceTracker.Application.Shops.Commands.UpdateShop;
using FinanceTracker.Application.Shops.DTOs;
using FinanceTracker.Application.Shops.Queries.GetShopById;
using FinanceTracker.Application.Shops.Queries.GetShops;
using FinanceTracker.Application.Shops.Queries.SearchShop;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/shops")]
public class ShopsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<PagedResult<ShopDto>>> GetAll(
        [FromQuery] GetShopsQuery query,
        CancellationToken cancellationToken
    )
    {
        var shops = await _mediator.Send(query, cancellationToken);
        return Ok(shops);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShopDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var shop = await _mediator.Send(new GetShopByIdQuery(id), cancellationToken);
        return Ok(shop);
    }

    // [Authorize(Roles="Admin")]
    [HttpPost]
    public async Task<ActionResult<ShopDto>> Create(
        [FromBody] CreateShopCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var shop = await _mediator.Send(cmd, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = shop.Id }, shop);
    }

    // [Authorize(Roles="Admin")]
    [HttpPut]
    public async Task<ActionResult<ShopDto>> Update(
        [FromBody] UpdateShopCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var shop = await _mediator.Send(cmd, cancellationToken);
        return Ok(shop);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<SearchShopResultDto>>> Search(
        [FromQuery] string searchString,
        CancellationToken cancellationToken
    )
    {
        var shops = await _mediator.Send(new SearchShopQuery(searchString), cancellationToken);
        return Ok(shops);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteShopCommand(id), cancellationToken);
        return NoContent();
    }
}
