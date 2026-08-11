using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Expenses.DTOs;

public record GetExpensesByUserResultDto(
    Guid Id,
    Guid? ShopId,
    string? ShopName,
    decimal TotalAmount,
    Currency Currency,
    DateOnly ExpenseDate
);
