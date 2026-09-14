using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Shops.DTOs;
using FinanceTracker.Domain.Enums;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Providers;

public class ShopProvider(FinanceTrackerDbContext ctx) : IShopProvider
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<PagedResult<ShopDto>> GetAsync(
        int page,
        int pageSize,
        IReadOnlyCollection<Guid?>? retailersIds,
        IReadOnlyCollection<string>? countries,
        IReadOnlyCollection<string>? cities,
        CancellationToken cancellationToken
    )
    {
        var query = _ctx.Shops.AsNoTracking();

        if (retailersIds?.Count > 0)
        {
            query = query.Where(x => retailersIds.Contains(x.RetailerId));
        }

        if (countries?.Count > 0)
        {
            query = query.Where(x => x.Address != null && countries.Contains(x.Address.Country));
        }

        if (cities?.Count > 0)
        {
            query = query.Where(x => x.Address != null && cities.Contains(x.Address.City));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var result = await query
            .OrderBy(x => x.Name)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ShopDto(
                x.Id,
                x.Name,
                x.RetailerId,
                x.Retailer != null ? x.Retailer.Name : null,
                x.Address != null ? x.Address.Country : null,
                x.Address != null ? x.Address.City : null
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ShopDto>(result, page, pageSize, totalCount);
    }
}
