using MediatR;

namespace FinanceTracker.Application.ExpenseDetails.Commands.DeleteExpenseDetail;

public record DeleteExpenseDetailCommand(
    Guid Id,
    Guid ExpenseId
) : IRequest;
