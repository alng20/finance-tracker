using FinanceTracker.Application.Shops.DTOs;

using MediatR;

namespace FinanceTracker.Application.Shops.Queries.SearchShop;

public record SearchShopQuery(string SearchString) : IRequest<IReadOnlyList<SearchShopResultDto>>;
