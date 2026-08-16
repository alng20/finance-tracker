using FinanceTracker.Application.Reports.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetRetailerAmountForPeriod;

/*
Returns the total expense amount for the specified shop and period
*/

public record GetRetailerAmountForPeriodQuery(
    DateOnly? FromDate,
    DateOnly? ToDate,
    Currency Currency
) : IRequest<GetRetailerAmountForPeriodResultDto>;
