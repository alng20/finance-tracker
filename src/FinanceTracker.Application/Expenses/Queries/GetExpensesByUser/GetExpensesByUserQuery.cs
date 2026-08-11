using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Expenses.DTOs;
using MediatR;

namespace FinanceTracker.Application.Expenses.Commands.GetExpensesByUser;

public record GetExpensesByUserQuery(int Page, int PageSize, DateOnly? FromDate, DateOnly? ToDate) : IRequest<PageResult<GetExpensesByUserResultDto>>;
