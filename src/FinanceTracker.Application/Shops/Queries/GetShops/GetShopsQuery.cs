using FinanceTracker.Application.Shops.DTOs;

using MediatR;

namespace FinanceTracker.Application.Shops.Queries.GetShops;

public record GetShopsQuery : IRequest<IReadOnlyList<ShopDto>>;
