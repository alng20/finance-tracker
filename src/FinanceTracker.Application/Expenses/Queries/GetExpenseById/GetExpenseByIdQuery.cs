using FinanceTracker.Application.Expenses.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Expenses.Commands.GetExpenseById;

public record GetExpenseByIdQuery(Guid Id) : IRequest<GetExpenseByIdResultDto>;
