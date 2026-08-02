using FinanceTracker.Application.Retailers.Commands.CreateRetailer;
using FinanceTracker.Application.Retailers.Commands.DeleteRetailer;
using FinanceTracker.Application.Retailers.Commands.UpdateRetailer;
using FinanceTracker.Application.Retailers.DTOs;
using FinanceTracker.Application.Retailers.Queries.GetRetailerById;
using FinanceTracker.Application.Retailers.Queries.GetRetailers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/retailers")]
public class RetailersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RetailerDto>>> GetAll(
        CancellationToken cancellationToken
    )
    {
        var retailers = await _mediator.Send(new GetRetailersQuery(), cancellationToken);
        return Ok(retailers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RetailerDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var retailer = await _mediator.Send(new GetRetailerByIdQuery(id), cancellationToken);
        return Ok(retailer);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<RetailerDto>> Create(
        [FromBody] CreateRetailerCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var retailer = await _mediator.Send(cmd, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = retailer.Id }, retailer);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult<RetailerDto>> Update(
        [FromBody] UpdateRetailerCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var retailer = await _mediator.Send(cmd, cancellationToken);
        return Ok(retailer);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteRetailerCommand(id), cancellationToken);
        return NoContent();
    }
}
