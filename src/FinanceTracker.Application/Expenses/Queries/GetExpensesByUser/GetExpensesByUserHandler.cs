using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using MediatR;

namespace FinanceTracker.Application.Expenses.Queries.GetExpensesByUser;

public class GetExpensesByUserHandler(
    ICurrentUserService currentUser,
    IExpenseProvider expenseProvider
) : IRequestHandler<GetExpensesByUserQuery, PageResult<GetExpensesByUserResultDto>>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IExpenseProvider _expenseProvider = expenseProvider;

    public async Task<PageResult<GetExpensesByUserResultDto>> Handle(
        GetExpensesByUserQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        return await _expenseProvider.GetByUserAsync(
            userId,
            query.Page,
            query.PageSize,
            query.FromDate,
            query.ToDate,
            cancellationToken
        );
    }
}
