using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Expenses.DTOs;

public record ExpenseDto(
    Guid Id,
    Guid UserId,
    Guid? SharedGroupId,
    Guid? ShopId,
    decimal TotalAmount,
    Currency Currency,
    DateTime ExpenseDate
);
