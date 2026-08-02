using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Shops.DTOs;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.ValueObjects;
using MediatR;

namespace FinanceTracker.Application.Shops.Commands.CreateShop;

public class CreateShopHandler(
    IShopRepository shopRepository,
    IRetailerRepository retailerRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateShopCommand, ShopDto>
{
    private readonly IShopRepository _shopRepository = shopRepository;
    private readonly IRetailerRepository _retailerRepository = retailerRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ShopDto> Handle(CreateShopCommand cmd, CancellationToken cancellationToken)
    {
        var shopWithName = await _shopRepository.FindByNameAsync(cmd.Name, cancellationToken);
        if (shopWithName != null)
        {
            throw new ConflictException($"Shop with name '{shopWithName.Name}' already exists");
        }

        Retailer? retailer = null;
        if (cmd.RetailerId.HasValue)
        {
            retailer = await _retailerRepository.GetByIdAsync(
                cmd.RetailerId.Value,
                cancellationToken
            );
        }

        Address? address =
            cmd.Country != null && cmd.City != null ? new Address(cmd.Country, cmd.City) : null;

        Shop shop = Shop.Create(cmd.Name, retailer?.Id, address);
        _shopRepository.Add(shop);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ShopDto(shop.Id, shop.Name, retailer?.Id, retailer?.Name, shop.Address?.Country, shop.Address?.City);
    }
}
