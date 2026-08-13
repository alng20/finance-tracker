namespace FinanceTracker.Application.Reports.DTOs;

public record ShopAmountDto(Guid? ShopId, string ShopName, decimal TotalAmount);
