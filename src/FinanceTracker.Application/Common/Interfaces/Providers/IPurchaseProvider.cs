using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;
using FinanceTracker.Application.Purchases.Enums;

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

    Task<PagedResult<GetItemPurchasesByIdResultDto>> GetPricesByIdAsync(
        Guid itemId,
        Guid userId,
        int page,
        int pageSize,
        DateOnly? fromDate,
        DateOnly? toDate,
        PricesSortType sortType,
        CancellationToken cancellationToken
    );
}
