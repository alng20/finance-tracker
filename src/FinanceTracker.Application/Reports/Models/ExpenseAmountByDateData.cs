using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Reports.Models;

public record ExpenseAmountByDateData(
    DateOnly Date,
    decimal Amount,
    Currency Currency
);
