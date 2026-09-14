using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Expenses.DTOs;
using MediatR;

namespace FinanceTracker.Application.Expenses.Queries.GetExpenseById;

public class GetExpenseByIdHandler(
    ICurrentUserService currentUser,
    IExpenseRepository expenseRepository
) : IRequestHandler<GetExpenseByIdQuery, GetExpenseByIdResultDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IExpenseRepository _expenseRepository = expenseRepository;

    public async Task<GetExpenseByIdResultDto> Handle(
        GetExpenseByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var expense = await _expenseRepository.GetByIdWithInfoNoTrackingAsync(
            query.Id,
            cancellationToken
        );
        // TODO: Check if expense.UserId have access to shared group
        if (userId != expense.UserId)
        {
            throw new UnauthorizedException("No access to expense");
        }

        var detailedAmount = expense.Details.Sum(x => x.TotalPrice.Amount);
        var undetailedAmount = expense.TotalAmount.Amount - detailedAmount;

        return new GetExpenseByIdResultDto(
            expense.Id,
            expense.UserId,
            expense.SharedGroupId,
            expense.ShopId,
            expense.Shop?.Name,
            expense.TotalAmount.Amount,
            expense.TotalAmount.Currency,
            expense.Date,
            expense
                .Details.Select(x => new GetExpenseByIdDetailResultDto(
                    x.Id,
                    x.ItemId,
                    x.Item.Name,
                    x.Item.Category.Id,
                    x.Item.Category.Name,
                    x.Item.Unit,
                    x.TotalPrice.Amount,
                    x.Quantity,
                    x.DiscountPercent,
                    x.UnitPrice.Amount,
                    x.UnitDiscountPrice?.Amount
                ))
                .ToList(),
            detailedAmount,
            undetailedAmount
        );
    }
}
