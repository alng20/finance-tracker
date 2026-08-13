using FinanceTracker.Application.Common.Enums;

namespace FinanceTracker.Application.Common.Models;

public record Discount(decimal Value, DiscountType Type);
