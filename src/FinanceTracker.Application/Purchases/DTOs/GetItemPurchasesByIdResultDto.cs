using FinanceTracker.Application.Purchases.Models;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Purchases.DTOs;

public record GetItemPurchasesByIdResultDto(
    decimal Price,
    Currency Currency,
    DateOnly Date,
    Guid? ShopId,
    string ShopName
) : PriceInfo(Price, Currency, Date, ShopId, ShopName);
