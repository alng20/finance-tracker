using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;
using MediatR;

namespace FinanceTracker.Application.Purchases.Queries.GetItemPurchasesById;

public class GetItemPurchasesByIdHandler(
    ICurrentUserService currentUser,
    IPurchaseProvider purchaseProvider
) : IRequestHandler<GetItemPurchasesByIdQuery, PagedResult<GetItemPurchasesByIdResultDto>>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IPurchaseProvider _purchaseProvider = purchaseProvider;

    // TODO: Support different currencies
    // private readonly ICurrencyConverter _currencyConverter = currencyConverter;

    public async Task<PagedResult<GetItemPurchasesByIdResultDto>> Handle(
        GetItemPurchasesByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var prices = await _purchaseProvider.GetPricesByIdAsync(
            query.Id,
            userId,
            query.Page,
            query.PageSize,
            query.FromDate,
            query.ToDate,
            cancellationToken
        );

        return prices;
    }
}
