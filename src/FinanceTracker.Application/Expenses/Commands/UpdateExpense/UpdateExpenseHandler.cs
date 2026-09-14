using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Domain.ValueObjects;

using MediatR;

namespace FinanceTracker.Application.Expenses.Commands.UpdateExpense;

public class UpdateExpenseHandler(
    ICurrentUserService currentUser,
    IExpenseRepository expenseRepository,
    IShopRepository shopRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateExpenseCommand, ExpenseResultDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IExpenseRepository _expenseRepository = expenseRepository;
    private readonly IShopRepository _shopRepository = shopRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ExpenseResultDto> Handle(
        UpdateExpenseCommand cmd,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var expense = await _expenseRepository.GetByIdWithDetailsAsync(cmd.Id, cancellationToken);
        // TODO: Check if expense.UserId have access to shared group
        if (userId != expense.UserId)
        {
            throw new UnauthorizedException("No access to expense");
        }

        if (cmd.ShopId is Guid shopId)
        {
            if (!await _shopRepository.ExistAsync(shopId, cancellationToken))
            {
                throw new NotFoundException($"Shop with id {shopId} was not found");
            }
        }
        // TODO: Check if SharedGroupId

        expense.Update(cmd.ShopId, cmd.SharedGroupId, Money.Create(cmd.TotalAmount, cmd.Currency), cmd.ExpenseDate);

        var detailedAmount = expense.Details.Sum(x => x.TotalPrice.Amount);
        var undetailedAmount = expense.TotalAmount.Amount - detailedAmount;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ExpenseResultDto(
            expense.Id,
            expense.UserId,
            expense.SharedGroupId,
            expense.ShopId,
            expense.TotalAmount.Amount,
            expense.TotalAmount.Currency,
            expense.Date,
            detailedAmount,
            undetailedAmount
        );
    }
}
