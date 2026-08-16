using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Reports.Models;

public record ExpenseRetailerAmountData(Guid? RetailerId, string RetailerName, decimal TotalAmount, Currency Currency);
