using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Items.DTOs;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Providers;

public class ItemProvider(FinanceTrackerDbContext ctx) : IItemProvider
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<PagedResult<GetItemsResultDto>> GetAsync(
        int page,
        int pageSize,
        IReadOnlyCollection<Guid>? categoryIds,
        IReadOnlyCollection<Unit>? units,
        CancellationToken cancellationToken
    )
    {
        var query = _ctx.Items.AsNoTracking();

        if (categoryIds?.Count > 0)
        {
            query = query.Where(x => categoryIds.Contains(x.CategoryId));
        }

        if (units?.Count > 0)
        {
            query = query.Where(x => units.Contains(x.Unit));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var result = await query
            .OrderBy(x => x.Name)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new GetItemsResultDto(
                x.Id,
                x.Name,
                x.CategoryId,
                x.Category != null ? x.Category.Name : null,
                x.Unit
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetItemsResultDto>(result, page, pageSize, totalCount);
    }
}
