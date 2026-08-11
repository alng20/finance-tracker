using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Api.Requests;

public record UpdateExpenseRequest(
    Guid? SharedGroupId,
    Guid? ShopId,
    decimal TotalAmount,
    Currency Currency,
    DateOnly ExpenseDate
);
