namespace FinanceTracker.Application.Reports.DTOs;

public record RetailerAmountDto(Guid? RetailerId, string RetailerName, decimal TotalAmount);
