using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using MediatR;

namespace FinanceTracker.Application.Expenses.Commands.DeleteExpense;

public class DeleteExpenseHandler(
    ICurrentUserService currentUser,
    IExpenseRepository expenseRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteExpenseCommand>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IExpenseRepository _expenseRepository = expenseRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(
        DeleteExpenseCommand cmd,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var expense = await _expenseRepository.GetByIdAsync(cmd.Id, cancellationToken);
        // TODO: Check if expense.UserId have access to shared group
        if (userId != expense.UserId)
        {
            throw new UnauthorizedException("No access to expense");
        }

        _expenseRepository.Delete(expense);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
