using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Shops.DTOs;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.ValueObjects;
using MediatR;

namespace FinanceTracker.Application.Shops.Commands.UpdateShop;

public class UpdateShopHandler(
    IShopRepository shopRepository,
    IRetailerRepository retailerRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateShopCommand, ShopDto>
{
    private readonly IShopRepository _shopRepository = shopRepository;
    private readonly IRetailerRepository _retailerRepository = retailerRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ShopDto> Handle(UpdateShopCommand cmd, CancellationToken cancellationToken)
    {
        Shop shop = await _shopRepository.GetByIdAsync(cmd.Id, cancellationToken);
        if (shop.Name == cmd.Name)
        {
            return new ShopDto(
                shop.Id,
                shop.Name,
                shop.Retailer?.Id,
                shop.Retailer?.Name,
                shop.Address?.Country,
                shop.Address?.City
            );
        }

        Shop? shopWithSameName = await _shopRepository.FindByNameAsync(cmd.Name, cancellationToken);
        if (shopWithSameName != null && shopWithSameName.Id != shop.Id)
        {
            throw new ConflictException($"Shop with name '{shopWithSameName.Name}' already exists");
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

        shop.Update(cmd.Name, retailer?.Id, address);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ShopDto(
            shop.Id,
            shop.Name,
            shop.Retailer?.Id,
            shop.Retailer?.Name,
            shop.Address?.Country,
            shop.Address?.City
        );
    }
}
