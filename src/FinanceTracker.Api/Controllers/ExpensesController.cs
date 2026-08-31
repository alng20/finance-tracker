using FinanceTracker.Api.Requests;
using FinanceTracker.Application.Expenses.Commands.CreateExpense;
using FinanceTracker.Application.Expenses.Commands.DeleteExpense;
using FinanceTracker.Application.Expenses.Commands.GetExpenseById;
using FinanceTracker.Application.Expenses.Commands.GetExpensesByUser;
using FinanceTracker.Application.Expenses.Commands.UpdateExpense;
using FinanceTracker.Application.Expenses.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.Application.Common.Models;

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
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default
    )
    {
        var expenses = await _mediator.Send(
            new GetExpensesByUserQuery(page, pageSize, fromDate, toDate),
            cancellationToken
        );
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
