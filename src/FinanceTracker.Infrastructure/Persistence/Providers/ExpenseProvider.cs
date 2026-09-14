using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Domain.Enums;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Providers;

public class ExpenseProvider(FinanceTrackerDbContext ctx) : IExpenseProvider
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<PagedResult<GetExpensesByUserResultDto>> GetByUserAsync(
        Guid userId,
        int page,
        int pageSize,
        DateOnly? fromDate,
        DateOnly? toDate,
        IReadOnlyCollection<Guid?>? shopIds,
        IReadOnlyCollection<Guid>? categoryIds,
        IReadOnlyCollection<Guid>? itemIds,
        IReadOnlyCollection<Guid?>? retailerIds,
        Currency? currency,
        decimal? fromAmount,
        decimal? toAmount,
        CancellationToken cancellationToken
    )
    {
        var query = _ctx.Expenses.AsNoTracking().Where(x => x.UserId == userId);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.Date >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.Date <= toDate.Value);
        }

        if (shopIds?.Count > 0)
        {
            query = query.Where(x => shopIds.Contains(x.ShopId));
        }

        if (categoryIds?.Count > 0)
        {
            query = query.Where(x =>
                x.Details.Any(detail => categoryIds.Contains(detail.Item.CategoryId))
            );
        }

        if (itemIds?.Count > 0)
        {
            query = query.Where(x => x.Details.Any(detail => itemIds.Contains(detail.ItemId)));
        }

        if (retailerIds?.Count > 0)
        {
            query = query.Where(x => x.Shop != null && retailerIds.Contains(x.Shop.RetailerId));
        }

        if (currency.HasValue)
        {
            query = query.Where(x => x.TotalAmount.Currency == currency);
        }

        if (fromAmount.HasValue)
        {
            query = query.Where(x => x.TotalAmount.Amount >= fromAmount.Value);
        }

        if (toAmount.HasValue)
        {
            query = query.Where(x => x.TotalAmount.Amount <= toAmount.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var result = await query
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new GetExpensesByUserResultDto(
                x.Id,
                x.ShopId,
                x.Shop != null ? x.Shop.Name : null,
                x.TotalAmount.Amount,
                x.TotalAmount.Currency,
                x.Date
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetExpensesByUserResultDto>(result, page, pageSize, totalCount);
    }
}
