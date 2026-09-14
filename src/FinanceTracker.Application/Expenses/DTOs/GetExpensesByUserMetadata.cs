using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Expenses.DTOs;

public record GetExpensesByUserMetadata(decimal SummaryAmount, Currency? Currency);
