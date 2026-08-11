using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.ExpenseDetails.DTOs;
using FinanceTracker.Domain.ValueObjects;
using MediatR;

namespace FinanceTracker.Application.ExpenseDetails.Commands.UpdateExpenseDetail;

public class UpdateExpenseDetailHandler(
    ICurrentUserService currentUser,
    IExpenseRepository expenseRepository,
    IItemRepository itemRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateExpenseDetailCommand, ExpenseDetailResultDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IExpenseRepository _expenseRepository = expenseRepository;
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ExpenseDetailResultDto> Handle(
        UpdateExpenseDetailCommand cmd,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var expense = await _expenseRepository.GetByIdWithDetailsAsync(cmd.ExpenseId, cancellationToken);
        // TODO: Check if expense.UserId have access to shared group
        if (userId != expense.UserId)
        {
            throw new UnauthorizedException("No access to expense");
        }

        if (!await _itemRepository.ExistAsync(cmd.ItemId, cancellationToken))
        {
            throw new NotFoundException($"Item with id {cmd.ItemId} was not found");
        }

        var detail = expense.UpdateDetail(
            cmd.Id,
            cmd.ItemId,
            Money.Create(cmd.TotalPrice, cmd.Currency),
            cmd.Quantity,
            cmd.DiscountPercent
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ExpenseDetailResultDto(
            detail.Id,
            detail.ExpenseId,
            detail.ItemId,
            detail.TotalPrice.Amount,
            detail.Quantity,
            detail.UnitPrice.Amount,
            detail.UnitDiscountPrice?.Amount,
            detail.TotalPrice.Currency,
            detail.DiscountPercent
        );
    }
}
