using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Domain.Enums;

using MediatR;

namespace FinanceTracker.Application.Expenses.Queries.GetExpenseById;

public record GetExpenseByIdQuery(Guid Id) : IRequest<GetExpenseByIdResultDto>;
