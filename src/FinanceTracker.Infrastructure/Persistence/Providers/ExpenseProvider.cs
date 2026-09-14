using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Application.Expenses.Enums;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Providers;

public class ExpenseProvider(FinanceTrackerDbContext ctx, ICurrencyConverter currencyConverter)
    : IExpenseProvider
{
    private readonly FinanceTrackerDbContext _ctx = ctx;
    private readonly ICurrencyConverter _currencyConverter = currencyConverter;

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

    public async Task<
        PagedResultWithMetadata<GetExpensesByUserResultDto, GetExpensesByUserMetadata>
    > GetByUserWithMetadataAsync(
        Guid userId,
        int page,
        int pageSize,
        ExpensesSortType? SortType,
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

        // TODO: Calculate different currencies or convert to UserSettings.Currency
        var summaryAmount = await query
            .Where(x => x.TotalAmount.Currency == Currency.NZD)
            .SumAsync(x => x.TotalAmount.Amount, cancellationToken);
        var totalCount = await query.CountAsync(cancellationToken);

        if (SortType == ExpensesSortType.Date)
        {
            query = query.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id);
        }

        if (SortType == ExpensesSortType.TotalAmount)
        {
            query = query.OrderByDescending(x => x.TotalAmount.Amount).ThenByDescending(x => x.Id);
        }

        if (SortType == ExpensesSortType.Shop)
        {
            query = query
                .OrderBy(x => x.Shop != null ? x.Shop.Name : "")
                .ThenByDescending(x => x.Id);
        }

        var result = await query
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

        return new PagedResultWithMetadata<GetExpensesByUserResultDto, GetExpensesByUserMetadata>(
            result,
            new GetExpensesByUserMetadata(summaryAmount, Currency.NZD),
            page,
            pageSize,
            totalCount
        );
    }
}
