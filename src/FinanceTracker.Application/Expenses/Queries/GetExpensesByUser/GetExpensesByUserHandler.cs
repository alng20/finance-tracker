using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;

using MediatR;

namespace FinanceTracker.Application.Expenses.Queries.GetExpensesByUser;

public class GetExpensesByUserHandler(
    ICurrentUserService currentUser,
    IExpenseProvider expenseProvider
) : IRequestHandler<GetExpensesByUserQuery, PagedResultWithMetadata<GetExpensesByUserResultDto, GetExpensesByUserMetadata>>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IExpenseProvider _expenseProvider = expenseProvider;

    public async Task<PagedResultWithMetadata<GetExpensesByUserResultDto, GetExpensesByUserMetadata>> Handle(
        GetExpensesByUserQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        // TODO: Check if Shops, Categories, Items, Retailers exists

        return await _expenseProvider.GetByUserWithMetadataAsync(
            userId,
            query.Page,
            query.PageSize,
            query.SortType,
            query.FromDate,
            query.ToDate,
            query.ShopIds,
            query.CategoryIds,
            query.ItemIds,
            query.RetailerIds,
            query.Currency,
            query.FromAmount,
            query.ToAmount,
            cancellationToken
        );
    }
}
