using MediatR;

namespace FinanceTracker.Application.Shops.Commands.DeleteShop;

public record DeleteShopCommand(Guid Id) : IRequest;
