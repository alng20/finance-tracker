using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Expenses.Commands.CreateExpense;

public record CreateExpenseDetail(
    Guid ItemId,
    decimal TotalPrice,
    decimal Quantity,
    decimal DiscountPercent
);
