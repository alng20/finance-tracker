using FinanceTracker.Application.ExpenseDetails.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.ExpenseDetails.Commands.CreateExpenseDetail;

public record CreateExpenseDetailCommand(
    Guid ExpenseId,
    Guid ItemId,
    decimal TotalPrice,
    Currency Currency,
    decimal Quantity,
    decimal DiscountPercent
) : IRequest<ExpenseDetailResultDto>;
