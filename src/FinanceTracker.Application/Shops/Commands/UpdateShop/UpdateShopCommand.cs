using FinanceTracker.Application.Shops.DTOs;

using MediatR;

namespace FinanceTracker.Application.Shops.Commands.UpdateShop;

public record UpdateShopCommand(
    Guid Id,
    string Name,
    Guid? RetailerId,
    string? Country,
    string? City
) : IRequest<ShopDto>;
