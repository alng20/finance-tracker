using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Application.Expenses.Enums;
using FinanceTracker.Domain.Enums;

using MediatR;

namespace FinanceTracker.Application.Expenses.Queries.GetExpensesByUser;

public record GetExpensesByUserQuery(
    int Page = 1,
    int PageSize = 20,
    ExpensesSortType? SortType = ExpensesSortType.Date,
    DateOnly? FromDate = null,
    DateOnly? ToDate = null,
    IReadOnlyCollection<Guid?>? ShopIds = null,
    IReadOnlyCollection<Guid>? CategoryIds = null,
    IReadOnlyCollection<Guid>? ItemIds = null,
    IReadOnlyCollection<Guid?>? RetailerIds = null,
    Currency? Currency = null,
    decimal? FromAmount = null,
    decimal? ToAmount = null
) : IRequest<PagedResultWithMetadata<GetExpensesByUserResultDto, GetExpensesByUserMetadata>>;
