using FinanceTracker.Application.Expenses.Commands.CreateExpense;
using FinanceTracker.Application.Expenses.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/expenses")]
public class ExpenseController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
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
