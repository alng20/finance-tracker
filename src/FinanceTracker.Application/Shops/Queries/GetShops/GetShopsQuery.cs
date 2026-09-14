using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Shops.DTOs;

using MediatR;

namespace FinanceTracker.Application.Shops.Queries.GetShops;

public record GetShopsQuery(
    int Page = 1,
    int PageSize = 20,
    IReadOnlyCollection<Guid?>? RetailersIds = null,
    IReadOnlyCollection<string>? Countries = null,
    IReadOnlyCollection<string>? Cities = null
) : IRequest<PagedResult<ShopDto>>;
