using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Domain.Enums;

using MediatR;

namespace FinanceTracker.Application.Expenses.Commands.CreateExpense;

public record CreateExpenseCommand(
    Guid? SharedGroupId,
    Guid? ShopId,
    decimal TotalAmount,
    Currency Currency,
    DateTime ExpenseDate,
    IReadOnlyList<CreateExpenseDetail> Details
) : IRequest<ExpenseDto>;
