namespace FinanceTracker.Application.Reports.DTOs;

public record GetRetailerAmountForPeriodResultDto(
    DateOnly? FromDate,
    DateOnly? ToDate,
    decimal TotalAmount,
    IReadOnlyCollection<RetailerAmountDto> Amounts
);
