using MediatR;

namespace FinanceTracker.Application.Expenses.Commands.DeleteExpense;

public record DeleteExpenseCommand(Guid Id) : IRequest;
