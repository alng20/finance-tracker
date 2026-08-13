using FinanceTracker.Application.Common.Models;

namespace FinanceTracker.Application.Expenses.DTOs;

// TODO: Add OriginalPrice?
public record CreateExpenseDetailDto(
    Guid ItemId,
    decimal TotalPrice,
    decimal Quantity,
    Discount? Discount
);
