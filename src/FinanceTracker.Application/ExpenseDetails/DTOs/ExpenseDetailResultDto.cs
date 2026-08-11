using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.ExpenseDetails.DTOs;

public record ExpenseDetailResultDto(
    Guid Id,
    Guid ExpenseId,
    Guid ItemId,
    decimal TotalPrice,
    decimal Quantity,
    decimal UnitPrice,
    decimal? UnitDiscountPrice,
    Currency Currency,
    decimal DiscountPercent
);
