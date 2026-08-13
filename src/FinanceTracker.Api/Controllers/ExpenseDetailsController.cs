using FinanceTracker.Api.Requests;
using FinanceTracker.Application.ExpenseDetails.Commands.CreateExpenseDetail;
using FinanceTracker.Application.ExpenseDetails.Commands.DeleteExpenseDetail;
using FinanceTracker.Application.ExpenseDetails.Commands.UpdateExpenseDetail;
using FinanceTracker.Application.ExpenseDetails.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/expenses/details")]
public class ExpenseDetailController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost("{expenseId}")]
    public async Task<ActionResult<ExpenseDetailResultDto>> Create(
        [FromRoute] Guid expenseId,
        [FromBody] CreateExpenseDetailRequest req,
        CancellationToken cancellationToken
    )
    {
        var expense = await _mediator.Send(
            new CreateExpenseDetailCommand(
                expenseId,
                req.ItemId,
                req.TotalPrice,
                req.Currency,
                req.Quantity,
                req.Discount
            ),
            cancellationToken
        );
        return Ok(expense);
    }

    [Authorize]
    [HttpPut("{expenseId}/{detailId}")]
    public async Task<ActionResult<ExpenseDetailResultDto>> Update(
        [FromRoute] Guid expenseId,
        [FromRoute] Guid detailId,
        [FromBody] UpdateExpenseDetailRequest req,
        CancellationToken cancellationToken
    )
    {
        var expense = await _mediator.Send(
            new UpdateExpenseDetailCommand(
                detailId,
                expenseId,
                req.ItemId,
                req.TotalPrice,
                req.Currency,
                req.Quantity,
                req.Discount
            ),
            cancellationToken
        );
        return Ok(expense);
    }

    [Authorize]
    [HttpDelete("{expenseId}/{detailId}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid expenseId,
        [FromRoute] Guid detailId,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(
            new DeleteExpenseDetailCommand(detailId, expenseId),
            cancellationToken
        );
        return NoContent();
    }
}
