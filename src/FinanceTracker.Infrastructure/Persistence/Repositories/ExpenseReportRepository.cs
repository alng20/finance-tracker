using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Reports.Enums;
using FinanceTracker.Application.Reports.Models;
using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

// TODO: Move to ExpenseProvider? 
public class ExpenseReportRepository(FinanceTrackerDbContext ctx) : IExpenseReportRepository
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<IReadOnlyCollection<ExpenseAmountByDateData>> GetTotalAmountAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    )
    {
        var query = GetUserExpensesForPeriod(userId, fromDate, toDate);

        return await query
            .Select(x => new ExpenseAmountByDateData(
                x.Date,
                x.TotalAmount.Amount,
                x.TotalAmount.Currency
            ))
            .ToListAsync(cancellationToken);
        ;
    }

    public async Task<
        IReadOnlyCollection<ExpensesTotalWithDetailsData>
    > GetExpensesTotalWithDetailsAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    )
    {
        var query = GetUserExpensesForPeriod(userId, fromDate, toDate);

        var result = await query
            .Select(x => new ExpensesTotalWithDetailsData(
                x.TotalAmount.Amount,
                x.TotalAmount.Currency,
                x.Details.Select(d => new ExpenseDetailData(
                        d.Item.Category.Id,
                        d.Item.Category.Name,
                        d.TotalPrice.Amount,
                        d.TotalPrice.Currency
                    ))
                    .ToList()
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<IReadOnlyCollection<ExpenseAmountByDateData>> GetExpensesAmountByDateAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    )
    {
        var query = GetUserExpensesForPeriod(userId, fromDate, toDate);

        var result = await query
            .Select(x => new ExpenseAmountByDateData(
                x.Date,
                x.TotalAmount.Amount,
                x.TotalAmount.Currency
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<IReadOnlyCollection<ExpenseShopAmountData>> GetExpensesAmountByShopAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    )
    {
        var query = GetUserExpensesForPeriod(userId, fromDate, toDate);

        var result = await query
            .GroupBy(x => new
            {
                ShopId = x.ShopId,
                ShopName = x.Shop == null ? "Unknown" : x.Shop.Name,
                x.TotalAmount.Currency,
            })
            .Select(g => new ExpenseShopAmountData(
                g.Key.ShopId,
                g.Key.ShopName,
                g.Sum(x => x.TotalAmount.Amount),
                g.Key.Currency
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<
        IReadOnlyCollection<ExpenseRetailerAmountData>
    > GetExpensesAmountByRetailerAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    )
    {
        var query = GetUserExpensesForPeriod(userId, fromDate, toDate);

        var result = await query
            .GroupBy(x => new
            {
                RetailerId = x.Shop == null ? null : x.Shop.RetailerId,
                RetailerName = x.Shop == null
                    ? "Unknown"
                    : (x.Shop.Retailer == null ? "Unknown" : x.Shop.Retailer.Name),
                x.TotalAmount.Currency,
            })
            .Select(g => new ExpenseRetailerAmountData(
                g.Key.RetailerId,
                g.Key.RetailerName,
                g.Sum(x => x.TotalAmount.Amount),
                g.Key.Currency
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<IReadOnlyCollection<TotalAmountByPeriodData>> GetGroupedAmountByPeriodAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        ReportGroupingType groupingType,
        CancellationToken cancellationToken
    )
    {
        var query = GetUserExpensesForPeriod(userId, fromDate, toDate);

        return groupingType switch
        {
            ReportGroupingType.Day => await GetDayAmounts(query, cancellationToken),
            ReportGroupingType.Month => await GetMonthAmounts(query, cancellationToken),
            ReportGroupingType.Year => await GetYearAmounts(query, cancellationToken),

            _ => throw new NotFoundException("Report grouping type is unsupported"),
        };
    }

    private IQueryable<Expense> GetUserExpensesForPeriod(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate
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

        return query;
    }

    private async Task<IReadOnlyCollection<TotalAmountByPeriodData>> GetDayAmounts(
        IQueryable<Expense> query,
        CancellationToken cancellationToken
    )
    {
        // TODO: Return total amount for periods

        return await query
            .GroupBy(x => new { x.Date, x.TotalAmount.Currency })
            .OrderByDescending(x => x.Key.Date)
            .Select(g => new TotalAmountByPeriodData(
                new ReportPeriod(g.Key.Date, g.Key.Date),
                g.Sum(x => x.TotalAmount.Amount),
                g.Key.Currency
            ))
            .ToListAsync(cancellationToken);
    }

    private async Task<IReadOnlyCollection<TotalAmountByPeriodData>> GetMonthAmounts(
        IQueryable<Expense> query,
        CancellationToken cancellationToken
    )
    {
        return await query
            .GroupBy(x => new
            {
                x.Date.Month,
                x.Date.Year,
                x.TotalAmount.Currency,
            })
            .OrderByDescending(x => x.Key.Year)
            .ThenByDescending(x => x.Key.Month)
            .Select(g => new TotalAmountByPeriodData(
                ReportPeriod.GetMonthPeriod(g.Key.Year, g.Key.Month),
                g.Sum(x => x.TotalAmount.Amount),
                g.Key.Currency
            ))
            .ToListAsync(cancellationToken);
    }

    private async Task<IReadOnlyCollection<TotalAmountByPeriodData>> GetYearAmounts(
        IQueryable<Expense> query,
        CancellationToken cancellationToken
    )
    {
        return await query
            .GroupBy(x => new { x.Date.Year, x.TotalAmount.Currency })
            .OrderByDescending(x => x.Key.Year)
            .Select(g => new TotalAmountByPeriodData(
                ReportPeriod.GetYearPeriod(g.Key.Year),
                g.Sum(x => x.TotalAmount.Amount),
                g.Key.Currency
            ))
            .ToListAsync(cancellationToken);
    }
}
