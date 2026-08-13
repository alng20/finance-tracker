using FinanceTracker.Application.Common.Models;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Api.Requests;

public record UpdateExpenseDetailRequest(
    Guid ItemId,
    decimal TotalPrice,
    Currency Currency,
    decimal Quantity,
    Discount? Discount
);
