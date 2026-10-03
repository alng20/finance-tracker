using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;

namespace FinanceTracker.Application.Common.Interfaces.Providers;

public interface IPurchaseProvider
{
    Task<PagedResult<GetItemPurchasesResultDto>> GetAsync(
        Guid userId,
        int page,
        int pageSize,
        DateOnly? fromDate,
        DateOnly? toDate,
        IReadOnlyCollection<Guid>? categoryIds,
        string? searchString,
        CancellationToken cancellationToken
    );

    // Task<PagedResult<GetPurchaseHistoryByIdResultDto>> GetByItemIdAsync(
    //     Guid userId,
    //     Guid itemId,
    //     int page,
    //     int pageSize,
    //     DateOnly? fromDate,
    //     DateOnly? toDate,
    //     CancellationToken cancellationToken
    // );
}
