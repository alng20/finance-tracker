using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;

using MediatR;

namespace FinanceTracker.Application.ExpenseDetails.Commands.DeleteExpenseDetail;

public class DeleteExpenseDetailHandler(
    ICurrentUserService currentUser,
    IExpenseRepository expenseRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteExpenseDetailCommand>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IExpenseRepository _expenseRepository = expenseRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteExpenseDetailCommand cmd, CancellationToken cancellationToken)
    {
        Guid userId = _currentUser.UserId;

        var expense = await _expenseRepository.GetByIdWithDetailsAsync(
            cmd.ExpenseId,
            cancellationToken
        );
        // TODO: Check if expense.UserId have access to shared group
        if (userId != expense.UserId)
        {
            throw new UnauthorizedException("No access to expense");
        }

        expense.DeleteDetail(cmd.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
