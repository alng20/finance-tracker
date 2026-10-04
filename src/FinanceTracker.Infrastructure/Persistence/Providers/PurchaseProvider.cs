using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;
using FinanceTracker.Application.Purchases.Models;
using FinanceTracker.Domain.Entities;
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
        string? searchString,
        CancellationToken cancellationToken
    )
    {
        var query = _ctx.Expenses.AsNoTracking();
        query = GetUserExpensesForPeriod(query, userId, fromDate, toDate);

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
                CategoryName = d.Item.Category.Name,
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

        if (!string.IsNullOrEmpty(searchString))
        {
            priceHistory = priceHistory.Where(x =>
                EF.Functions.ILike(x.ItemName, $"%{searchString}%")
            );
        }

        // TODO: Support different currencies (convert or get for query.currency?)
        priceHistory = priceHistory.Where(x => x.UnitPrice.Currency == Currency.NZD);

        var grouped = priceHistory.GroupBy(x => new
        {
            x.ItemId,
            x.ItemName,
            x.CategoryId,
            x.CategoryName,
            x.Unit,
        });
        var totalCount = await grouped.CountAsync(cancellationToken);

        var result = await grouped
            .OrderBy(g => g.Key.ItemName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(g => new GetItemPurchasesResultDto(
                g.Key.ItemId,
                g.Key.ItemName,
                g.Key.CategoryId,
                g.Key.CategoryName,
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
            .ToListAsync(cancellationToken);

        return new PagedResult<GetItemPurchasesResultDto>(result, page, pageSize, totalCount);
    }

    public async Task<PagedResult<GetItemPurchasesByIdResultDto>> GetPricesByIdAsync(
        Guid itemId,
        Guid userId,
        int page,
        int pageSize,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    )
    {
        var query = _ctx.Expenses.AsNoTracking();
        query = GetUserExpensesForPeriod(query, userId, fromDate, toDate);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.Date >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.Date <= toDate.Value);
        }
        // TODO: Support different currencies (convert or get for query.currency?)
        query = query.Where(x => x.TotalAmount.Currency == Currency.NZD);

        var priceHistory = query
            .SelectMany(x =>
                x.Details.Where(d => d.ItemId == itemId)
                    .Select(d => new
                    {
                        Price = d.UnitPrice.Amount,
                        Currency = d.UnitPrice.Currency,
                        Date = x.Date,
                        ShopId = x.ShopId,
                        ShopName = x.Shop != null ? x.Shop.Name : "Unknown",
                    })
            )
            .OrderByDescending(x => x.Date);

        var totalCount = await priceHistory.CountAsync(cancellationToken);
        var result = await priceHistory
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new GetItemPurchasesByIdResultDto(
                x.Price,
                x.Currency,
                x.Date,
                x.ShopId,
                x.ShopName
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetItemPurchasesByIdResultDto>(result, page, pageSize, totalCount);
    }

    private IQueryable<Expense> GetUserExpensesForPeriod(
        IQueryable<Expense> query,
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate
    )
    {
        query = query.Where(x => x.UserId == userId);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.Date >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.Date <= toDate.Value);
        }

        return query;
    }
}
