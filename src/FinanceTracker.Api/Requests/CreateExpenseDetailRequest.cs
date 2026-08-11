using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Api.Requests;

public record CreateExpenseDetailRequest(
    Guid ItemId,
    decimal TotalPrice,
    Currency Currency,
    decimal Quantity,
    decimal DiscountPercent
);
