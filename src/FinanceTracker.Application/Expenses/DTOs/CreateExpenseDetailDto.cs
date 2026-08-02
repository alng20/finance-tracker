namespace FinanceTracker.Application.Expenses.DTOs;

public record CreateExpenseDetailDto(
    Guid ItemId,
    decimal TotalPrice,
    decimal Quantity,
    decimal DiscountPercent
);
