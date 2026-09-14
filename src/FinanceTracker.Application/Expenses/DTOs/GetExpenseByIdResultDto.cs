using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Expenses.DTOs;

public record GetExpenseByIdResultDto(
    Guid Id,
    Guid UserId,
    Guid? SharedGroupId,
    Guid? ShopId,
    string? ShopName,
    decimal TotalAmount,
    Currency Currency,
    DateOnly ExpenseDate,
    IReadOnlyList<GetExpenseByIdDetailResultDto> Details,
    decimal DetailedAmount,
    decimal UndetailedAmount
);
