using FinanceTracker.Application.Purchases.Models;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Purchases.DTOs;

public record GetItemPurchasesResultDto(
    Guid Id,
    string Name,
    Guid CategoryId,
    string CategoryName,
    Unit unit,
    PriceInfo MinPrice,
    PriceInfo MaxPrice
);
