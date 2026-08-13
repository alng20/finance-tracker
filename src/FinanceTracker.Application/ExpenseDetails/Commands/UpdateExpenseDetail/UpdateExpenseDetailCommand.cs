using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.ExpenseDetails.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.ExpenseDetails.Commands.UpdateExpenseDetail;

public record UpdateExpenseDetailCommand(
    Guid Id,
    Guid ExpenseId,
    Guid ItemId,
    decimal TotalPrice,
    Currency Currency,
    decimal Quantity,
    Discount? Discount
) : IRequest<ExpenseDetailResultDto>;
