using FinanceTracker.Api.Requests;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.Commands.CreateExpense;
using FinanceTracker.Application.Expenses.Commands.DeleteExpense;
using FinanceTracker.Application.Expenses.Commands.UpdateExpense;
using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Application.Expenses.Queries.GetExpenseById;
using FinanceTracker.Application.Expenses.Queries.GetExpensesByUser;

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
    [HttpGet("{id}")]
    public async Task<ActionResult<GetExpenseByIdResultDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var expense = await _mediator.Send(new GetExpenseByIdQuery(id), cancellationToken);
        return Ok(expense);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PageResult<GetExpensesByUserResultDto>>> GetByUser(
        [FromQuery] GetExpensesByUserQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var expenses = await _mediator.Send(query, cancellationToken);
        return Ok(expenses);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ExpenseResultDto>> Create(
        [FromBody] CreateExpenseCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var expense = await _mediator.Send(cmd, cancellationToken);
        return Ok(expense);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ExpenseResultDto>> UpdateById(
        [FromRoute] Guid id,
        [FromBody] UpdateExpenseRequest req,
        CancellationToken cancellationToken
    )
    {
        var expense = await _mediator.Send(
            new UpdateExpenseCommand(
                id,
                req.SharedGroupId,
                req.ShopId,
                req.TotalAmount,
                req.Currency,
                req.ExpenseDate
            ),
            cancellationToken
        );
        return Ok(expense);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new DeleteExpenseCommand(id), cancellationToken);
        return NoContent();
    }
}
