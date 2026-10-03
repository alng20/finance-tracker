using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;
using FinanceTracker.Application.Purchases.Models;
using FinanceTracker.Domain.Enums;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Providers;

public class PurchaseProvider(FinanceTrackerDbContext ctx) : IPurchaseProvider
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<PagedResult<GetItemPurchasesResultDto>> GetAsync(
        Guid userId,
        int page,
        int pageSize,
        DateOnly? fromDate,
        DateOnly? toDate,
        IReadOnlyCollection<Guid>? categoryIds,
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

        var priceHistory = query.SelectMany(x =>
            x.Details.Select(d => new
            {
                ItemId = d.ItemId,
                ItemName = d.Item.Name,
                CategoryId = d.Item.CategoryId,
                Unit = d.Item.Unit,
                ShopId = x.ShopId,
                ShopName = x.Shop != null ? x.Shop.Name : "Unknown",
                UnitPrice = d.UnitPrice,
                Date = x.Date,
            })
        );

        if (categoryIds?.Count > 0)
        {
            priceHistory = priceHistory.Where(x => categoryIds.Contains(x.CategoryId));
        }

        // TODO: Support different currencies (convert or get for query.currency?)
        priceHistory = priceHistory.Where(x => x.UnitPrice.Currency == Currency.NZD);

        var grouped = priceHistory.GroupBy(x => new { x.ItemId, x.ItemName, x.Unit });
        var totalCount = await grouped.CountAsync();

        var result = await grouped
            .OrderBy(g => g.Key.ItemName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(g => new GetItemPurchasesResultDto(
                g.Key.ItemId,
                g.Key.ItemName,
                g.Key.Unit,
                g.OrderBy(x => x.UnitPrice.Amount)
                    .ThenBy(x => x.Date)
                    .Select(x => new PriceInfo(
                        x.UnitPrice.Amount,
                        x.UnitPrice.Currency,
                        x.Date,
                        x.ShopId,
                        x.ShopName
                    ))
                    .First(),
                g.OrderByDescending(x => x.UnitPrice.Amount)
                    .ThenBy(x => x.Date)
                    .Select(x => new PriceInfo(
                        x.UnitPrice.Amount,
                        x.UnitPrice.Currency,
                        x.Date,
                        x.ShopId,
                        x.ShopName
                    ))
                    .First()
            ))
            .ToListAsync();

        return new PagedResult<GetItemPurchasesResultDto>(result, page, pageSize, totalCount);
    }
}
