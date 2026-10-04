using FinanceTracker.Application.Common.Consts;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;
using FinanceTracker.Application.Purchases.Enums;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Purchases.Queries.GetItemPurchasesById;

public record GetItemPurchasesByIdQuery(
    Guid Id,
    Currency Currency,
    int Page = PageConsts.DefaultPage,
    int PageSize = PageConsts.DefaultPageSize,
    DateOnly? FromDate = null,
    DateOnly? ToDate = null,
    PricesSortType SortType = PricesSortType.DateDesc
) : MediatR.IRequest<PagedResult<GetItemPurchasesByIdResultDto>>;
