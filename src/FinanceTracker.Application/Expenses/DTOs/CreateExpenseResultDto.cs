using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Expenses.DTOs;

public record ExpenseResultDto(
    Guid Id,
    Guid UserId,
    Guid? SharedGroupId,
    Guid? ShopId,
    decimal TotalAmount,
    Currency Currency,
    DateOnly ExpenseDate,
    decimal DetailedAmount,
    decimal UndetailedAmount
);
