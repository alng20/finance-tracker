using FinanceTracker.Application.Common.Enums;
using FinanceTracker.Application.Common.Exceptions;
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
    IShopRepository shopRepository,
    IItemRepository itemRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateExpenseCommand, ExpenseResultDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IExpenseRepository _expenseRepository = expenseRepository;
    private readonly IShopRepository _shopRepository = shopRepository;
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ExpenseResultDto> Handle(
        CreateExpenseCommand cmd,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        if (cmd.ShopId is Guid shopId)
        {
            if (!await _shopRepository.ExistAsync(shopId, cancellationToken))
            {
                throw new NotFoundException($"Shop with id {shopId} was not found");
            }
        }

        // TODO: Check if SharedGroupId

        Money money = Money.Create(cmd.TotalAmount, cmd.Currency);

        Expense expense = Expense.Create(
            userId,
            cmd.SharedGroupId,
            cmd.ShopId,
            money,
            cmd.ExpenseDate
        );

        decimal detailedAmount = 0;
        foreach (var detail in cmd.Details)
        {
            if (!await _itemRepository.ExistAsync(detail.ItemId, cancellationToken))
            {
                throw new NotFoundException($"Item with id {detail.ItemId} was not found");
            }

            decimal discountPercent = 0;
            if (detail.Discount is not null)
            {
                // TODO: refactor with original price
                if (detail.Discount.Type == DiscountType.Amount)
                {
                    discountPercent = detail.Discount.Value / detail.TotalPrice * 100;
                }
                if (detail.Discount.Type == DiscountType.Percent)
                {
                    discountPercent = detail.Discount.Value;
                }
            }

            Money detailMoney = Money.Create(detail.TotalPrice, cmd.Currency);
            expense.AddDetail(detail.ItemId, detailMoney, detail.Quantity, discountPercent);

            detailedAmount += detail.TotalPrice;
        }

        var undetailedAmount = expense.TotalAmount.Amount - detailedAmount;

        _expenseRepository.Add(expense);
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
