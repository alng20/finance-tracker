using FinanceTracker.Application.Reports.DTOs;
using FinanceTracker.Domain.Enums;

using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetTotalAmountForPeriod;

public record GetTotalAmountForPeriodQuery(
    DateOnly? FromDate,
    DateOnly? ToDate,
    Currency Currency
) : IRequest<GetTotalAmountForPeriodResultDto>;
