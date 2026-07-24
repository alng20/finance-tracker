using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.ValueObjects;

using MediatR;

namespace FinanceTracker.Application.Expenses.Commands.CreateExpense;

public class CreateExpenseHandler(
    ICurrentUserService currentUser,
    IExpenseRepository expenseRepository,
    IItemRepository itemRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateExpenseCommand, ExpenseDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IExpenseRepository _expenseRepository = expenseRepository;
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ExpenseDto> Handle(
        CreateExpenseCommand cmd,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        Money money = Money.Create(cmd.TotalAmount, cmd.Currency);

        // TODO: Check if SharedGroupId and ShopId exists

        Expense expense = Expense.Create(
            userId,
            cmd.SharedGroupId,
            cmd.ShopId,
            money,
            cmd.ExpenseDate
        );

        foreach (var detail in cmd.Details)
        {
            Item item = await _itemRepository.GetByIdAsync(detail.ItemId, cancellationToken);

            Money detailMoney = Money.Create(detail.TotalPrice, cmd.Currency);
            expense.AddDetail(item.Id, detailMoney, detail.Quantity, detail.DiscountPercent);
        }

        _expenseRepository.Add(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ExpenseDto(
            expense.Id,
            expense.UserId,
            expense.SharedGroupId,
            expense.ShopId,
            expense.TotalAmount.Amount,
            expense.TotalAmount.Currency,
            expense.Date
        );
    }
}
