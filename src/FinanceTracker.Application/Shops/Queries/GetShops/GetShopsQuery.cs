using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Shops.DTOs;
using MediatR;

namespace FinanceTracker.Application.Shops.Queries.GetShops;

public record GetShopsQuery(
    int Page,
    int PageSize,
    IReadOnlyCollection<Guid?>? RetailersIds,
    IReadOnlyCollection<string>? Countries,
    IReadOnlyCollection<string>? Cities
) : IRequest<PagedResult<ShopDto>>;
