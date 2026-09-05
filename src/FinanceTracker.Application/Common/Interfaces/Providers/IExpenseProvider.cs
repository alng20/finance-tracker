using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Common.Interfaces.Providers;

public interface IExpenseProvider
{
    Task<PageResult<GetExpensesByUserResultDto>> GetByUserAsync(
        Guid userId,
        int page,
        int pageSize,
        DateOnly? fromDate,
        DateOnly? toDate,
        IReadOnlyCollection<Guid?>? ShopIds,
        IReadOnlyCollection<Guid>? CategoryIds,
        IReadOnlyCollection<Guid>? ItemIds,
        IReadOnlyCollection<Guid?>? RetailerIds,
        Currency? currency,
        decimal? FromAmount,
        decimal? ToAmount,
        CancellationToken cancellationToken
    );
}
