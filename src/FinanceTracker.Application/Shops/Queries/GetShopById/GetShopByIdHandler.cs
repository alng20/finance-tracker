using System.Xml.Linq;

using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Shops.DTOs;

using MediatR;

namespace FinanceTracker.Application.Shops.Queries.GetShopById;

public class GetShopHandler(IShopRepository shopRepository)
    : IRequestHandler<GetShopByIdQuery, ShopDto>
{
    private readonly IShopRepository _shopRepository = shopRepository;

    public async Task<ShopDto> Handle(
        GetShopByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var shop = await _shopRepository.GetByIdWithRetailerAsync(query.Id, cancellationToken);
        return new ShopDto(shop.Id, shop.Name, shop.Retailer?.Id, shop.Retailer?.Name, shop.Address?.Country, shop.Address?.City);
    }
}
