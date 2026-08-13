using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Reports.Models;

public record ExpensesTotalWithDetailsData(
    decimal TotalAmount,
    Currency Currency,
    IReadOnlyCollection<ExpenseDetailData> Details
);
