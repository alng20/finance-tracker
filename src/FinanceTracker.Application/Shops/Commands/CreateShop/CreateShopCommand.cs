using FinanceTracker.Application.Shops.DTOs;

using MediatR;

namespace FinanceTracker.Application.Shops.Commands.CreateShop;

public record CreateShopCommand(string Name, Guid? RetailerId, string? Country, string? City) : IRequest<ShopDto>;
