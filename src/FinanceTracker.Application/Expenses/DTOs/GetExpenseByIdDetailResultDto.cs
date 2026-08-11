namespace FinanceTracker.Application.Expenses.DTOs;

public record GetExpenseByIdDetailResultDto(
    Guid Id,
    Guid ItemId,
    string ItemName,
    decimal TotalPrice,
    decimal Quantity,
    decimal DiscountPercent,
    decimal UnitPrice,
    decimal? UnitDiscountPrice
);
