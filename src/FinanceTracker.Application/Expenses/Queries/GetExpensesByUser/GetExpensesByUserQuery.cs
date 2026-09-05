using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Domain.Enums;

using MediatR;

namespace FinanceTracker.Application.Expenses.Queries.GetExpensesByUser;

public record GetExpensesByUserQuery(
    int Page,
    int PageSize,
    DateOnly? FromDate,
    DateOnly? ToDate,
    IReadOnlyCollection<Guid?>? ShopIds,
    IReadOnlyCollection<Guid>? CategoryIds,
    IReadOnlyCollection<Guid>? ItemIds,
    IReadOnlyCollection<Guid?>? RetailerIds,
    Currency? Currency,
    decimal? FromAmount,
    decimal? ToAmount
) : IRequest<PageResult<GetExpensesByUserResultDto>>;
