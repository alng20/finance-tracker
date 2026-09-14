using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Shops.DTOs;

namespace FinanceTracker.Application.Common.Interfaces.Providers;

public interface IShopProvider
{
    Task<PagedResult<ShopDto>> GetAsync(
        int page,
        int pageSize,
        IReadOnlyCollection<Guid?>? retailersIds,
        IReadOnlyCollection<string>? countries,
        IReadOnlyCollection<string>? cities,
        CancellationToken cancellationToken
    );
}
