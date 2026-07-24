using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class ExpenseRepository(FinanceTrackerDbContext ctx) : IExpenseRepository
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public void Add(Expense expense)
    {
        _ctx.Expenses.Add(expense);
    }
}
