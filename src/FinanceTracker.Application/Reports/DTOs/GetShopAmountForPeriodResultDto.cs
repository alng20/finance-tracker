namespace FinanceTracker.Application.Reports.DTOs;

public record GetShopAmountForPeriodResultDto(
    DateOnly? FromDate,
    DateOnly? ToDate,
    decimal TotalAmount,
    IReadOnlyCollection<ShopAmountDto> Amounts
);
