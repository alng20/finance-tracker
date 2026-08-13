using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Reports.Models;

public record ExpenseDetailData(Guid CategoryId, string CategoryName, decimal TotalPrice, Currency Currency);
