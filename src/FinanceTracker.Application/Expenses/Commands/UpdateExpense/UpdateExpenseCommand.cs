using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Domain.Enums;

using MediatR;

namespace FinanceTracker.Application.Expenses.Commands.UpdateExpense;

public record UpdateExpenseCommand(
    Guid Id,
    Guid? SharedGroupId,
    Guid? ShopId,
    decimal TotalAmount,
    Currency Currency,
    DateOnly ExpenseDate
) : IRequest<ExpenseResultDto>;
