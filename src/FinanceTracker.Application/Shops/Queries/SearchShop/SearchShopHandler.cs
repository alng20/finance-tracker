using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Shops.DTOs;
using MediatR;

namespace FinanceTracker.Application.Shops.Queries.SearchShop;

public class SearchShopHandler(IShopRepository shopRepository)
    : IRequestHandler<SearchShopQuery, IReadOnlyList<SearchShopResultDto>>
{
    private readonly IShopRepository _shopRepository = shopRepository;

    public async Task<IReadOnlyList<SearchShopResultDto>> Handle(
        SearchShopQuery query,
        CancellationToken cancellationToken
    )
    {
        var shops = await _shopRepository.SearchByNameAsync(query.SearchString, cancellationToken);
        return shops
            .Select(x => new SearchShopResultDto(
                x.Id,
                x.Name,
                x.RetailerId,
                x.Retailer != null ? x.Retailer.Name : null,
                x.Address != null ? x.Address.Country : null,
                x.Address != null ? x.Address.City : null
            ))
            .ToList();
    }
}
