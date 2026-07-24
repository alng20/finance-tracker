using FinanceTracker.Application.Expenses.Commands.CreateExpense;
using FinanceTracker.Application.Expenses.DTOs;

using MediatR;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/expenses")]
public class ExpenseController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create(
        [FromBody] CreateExpenseCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var item = await _mediator.Send(cmd, cancellationToken);
        return Ok(item);
    }
}
