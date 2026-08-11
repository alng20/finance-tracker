using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Providers;

public class ExpenseProvider(FinanceTrackerDbContext ctx) : IExpenseProvider
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<PageResult<GetExpensesByUserResultDto>> GetByUserAsync(
        Guid userId,
        int page,
        int pageSize,
        DateOnly? fromDate,
        DateOnly? toDate,
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

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
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

        return new PageResult<GetExpensesByUserResultDto>(items, page, pageSize, totalCount);
    }
}
