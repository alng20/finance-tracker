namespace FinanceTracker.Application.Reports.DTOs;

public record CategoryAmountDto(Guid CategoryId, string CategoryName, decimal TotalAmount);
