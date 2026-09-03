using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using MediatR;

namespace FinanceTracker.Application.Expenses.Queries.GetExpensesByUser;

public record GetExpensesByUserQuery(int Page, int PageSize, DateOnly? FromDate, DateOnly? ToDate) : IRequest<PageResult<GetExpensesByUserResultDto>>;
