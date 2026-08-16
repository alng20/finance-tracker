using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Shops.DTOs;
using MediatR;

namespace FinanceTracker.Application.Shops.Queries.GetShops;

public class GetShopsHandler(IShopRepository categoryRepository)
    : IRequestHandler<GetShopsQuery, IReadOnlyList<ShopDto>>
{
    private readonly IShopRepository _shopRepository = categoryRepository;

    public async Task<IReadOnlyList<ShopDto>> Handle(
        GetShopsQuery query,
        CancellationToken cancellationToken
    )
    {
        var shops = await _shopRepository.GetAllAsync(cancellationToken);
        return shops
            .Select(x => new ShopDto(
                x.Id,
                x.Name,
                x.Retailer?.Id,
                x.Retailer?.Name,
                x.Address?.Country,
                x.Address?.City
            ))
            .ToList();
    }
}
