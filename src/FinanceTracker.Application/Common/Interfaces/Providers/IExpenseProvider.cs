using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Application.Expenses.Enums;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Common.Interfaces.Providers;

public interface IExpenseProvider
{
    Task<PagedResult<GetExpensesByUserResultDto>> GetByUserAsync(
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

    Task<PagedResultWithMetadata<GetExpensesByUserResultDto, GetExpensesByUserMetadata>> GetByUserWithMetadataAsync(
        Guid userId,
        int page,
        int pageSize,
        ExpensesSortType? sortType,
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
