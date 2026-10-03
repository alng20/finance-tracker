using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;

using MediatR;

namespace FinanceTracker.Application.Purchases.Queries.GetItemPurchases;

public class GetItemPurchasesHandler(
    ICurrentUserService currentUser,
    IPurchaseProvider purchaseProvider
) : IRequestHandler<GetItemPurchasesQuery, PagedResult<GetItemPurchasesResultDto>>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IPurchaseProvider _purchaseProvider = purchaseProvider;

    // TODO: Support different currencies
    // private readonly ICurrencyConverter _currencyConverter = currencyConverter;

    public async Task<PagedResult<GetItemPurchasesResultDto>> Handle(
        GetItemPurchasesQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var purchases = await _purchaseProvider.GetAsync(
            userId,
            query.Page,
            query.PageSize,
            query.FromDate,
            query.ToDate,
            query.CategoryIds,
            cancellationToken
        );

        return purchases;
    }
}
