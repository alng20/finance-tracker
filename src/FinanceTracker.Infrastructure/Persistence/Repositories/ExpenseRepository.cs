using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class ExpenseRepository(FinanceTrackerDbContext ctx) : IExpenseRepository
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<Expense> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Expense? expense = await _ctx.Expenses.FindAsync(new object[] { id }, cancellationToken);
        return expense ?? throw new NotFoundException($"Expense with id {id} was not found");
    }

    public async Task<Expense> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        var expense = await _ctx
            .Expenses.Include(x => x.Details)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return expense ?? throw new NotFoundException($"Expense with id {id} was not found");
    }

    public async Task<Expense> GetByIdWithInfoNoTrackingAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var expense = await _ctx
            .Expenses.AsNoTracking()
            .Include(x => x.Shop)
            .Include(x => x.Details)
                .ThenInclude(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return expense ?? throw new NotFoundException($"Expense with id {id} was not found");
    }

    public void Add(Expense expense)
    {
        _ctx.Expenses.Add(expense);
    }

    public void Delete(Expense expense)
    {
        _ctx.Expenses.Remove(expense);
    }
}
