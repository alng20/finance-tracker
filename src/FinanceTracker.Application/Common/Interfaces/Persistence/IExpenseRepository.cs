using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IExpenseRepository
{
    void Add(Expense expense);
}
