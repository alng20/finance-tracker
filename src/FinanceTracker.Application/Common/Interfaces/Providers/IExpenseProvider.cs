using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;

namespace FinanceTracker.Application.Common.Interfaces.Providers;

public interface IExpenseProvider
{
    Task<PageResult<GetExpensesByUserResultDto>> GetByUserAsync(
        Guid userId,
        int page,
        int pageSize,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    );
}
