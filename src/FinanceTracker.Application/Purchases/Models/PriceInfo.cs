using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Purchases.Models;

public record PriceInfo(
    decimal Price,
    Currency Currency,
    DateOnly Date,
    Guid? ShopId,
    string ShopName
);
