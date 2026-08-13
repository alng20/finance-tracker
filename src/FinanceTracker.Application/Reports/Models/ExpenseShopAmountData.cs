using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Reports.Models;

public record ExpenseShopAmountData(Guid? ShopId, string ShopName, decimal TotalAmount, Currency Currency);
