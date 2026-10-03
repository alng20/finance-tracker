using FinanceTracker.Application.Common.Consts;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Purchases.Queries.GetItemPurchases;

public record GetItemPurchasesQuery(
    Currency Currency,
    int Page = PageConsts.DefaultPage,
    int PageSize = PageConsts.DefaultPageSize,
    DateOnly? FromDate = null,
    DateOnly? ToDate = null,
    IReadOnlyCollection<Guid>? CategoryIds = null,
    string? SearchString = null
) : MediatR.IRequest<PagedResult<GetItemPurchasesResultDto>>;
