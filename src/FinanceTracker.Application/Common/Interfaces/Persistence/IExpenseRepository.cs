using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IExpenseRepository
{
    Task<Expense> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Expense> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    Task<Expense> GetByIdWithInfoNoTrackingAsync(Guid id, CancellationToken cancellationToken);
    void Add(Expense expense);
    void Delete(Expense expense);
}
