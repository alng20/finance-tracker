using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Expenses.DTOs;

public record GetExpenseByIdDetailResultDto(
    Guid Id,
    Guid ItemId,
    string ItemName,
    string CategoryName,
    Unit Unit,
    decimal TotalPrice,
    decimal Quantity,
    decimal DiscountPercent,
    decimal UnitPrice,
    decimal? UnitDiscountPrice
);
