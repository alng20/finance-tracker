using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;
using FinanceTracker.Application.Purchases.Queries.GetItemPurchases;
using FinanceTracker.Application.Purchases.Queries.GetItemPurchasesById;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/purchases")]
public class PurchasesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResult<GetItemPurchasesResultDto>>> Get(
        [FromQuery] GetItemPurchasesQuery query,
        CancellationToken cancellationToken
    )
    {
        var purchases = await _mediator.Send(query, cancellationToken);
        return Ok(purchases);
    }


    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<PagedResult<GetItemPurchasesByIdResultDto>>> GetPricesById(
        [FromRoute] Guid id,
        [FromQuery] GetItemPurchasesByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        query = query with { Id = id };
        var purchases = await _mediator.Send(query, cancellationToken);
        return Ok(purchases);
    }
}
