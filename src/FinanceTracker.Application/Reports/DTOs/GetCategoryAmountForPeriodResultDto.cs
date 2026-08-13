namespace FinanceTracker.Application.Reports.DTOs;

public record GetCategoryAmountForPeriodResultDto(
    DateOnly? FromDate,
    DateOnly? ToDate,
    decimal TotalAmount,
    IReadOnlyCollection<CategoryAmountDto> Amounts,
    decimal DetailedAmount,
    decimal UndetailedAmount
);
