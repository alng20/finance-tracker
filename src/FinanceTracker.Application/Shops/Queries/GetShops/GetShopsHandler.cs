using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Shops.DTOs;

using MediatR;

namespace FinanceTracker.Application.Shops.Queries.GetShops;

public class GetShopsHandler(IShopProvider shopProvider)
    : IRequestHandler<GetShopsQuery, PagedResult<ShopDto>>
{
    private readonly IShopProvider _shopProvider = shopProvider;

    public async Task<PagedResult<ShopDto>> Handle(
        GetShopsQuery query,
        CancellationToken cancellationToken
    )
    {
        return await _shopProvider.GetAsync(
            query.Page,
            query.PageSize,
            query.RetailersIds,
            query.Countries,
            query.Cities,
            cancellationToken
        );
    }
}
