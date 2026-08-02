using FinanceTracker.Application.Shops.DTOs;

using MediatR;

namespace FinanceTracker.Application.Shops.Queries.GetShopById;

public record GetShopByIdQuery(Guid Id) : IRequest<ShopDto>;
