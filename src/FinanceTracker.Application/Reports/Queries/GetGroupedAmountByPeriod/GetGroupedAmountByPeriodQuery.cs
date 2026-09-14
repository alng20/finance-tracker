using FinanceTracker.Application.Reports.DTOs;
using FinanceTracker.Application.Reports.Enums;
using FinanceTracker.Domain.Enums;

using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetGroupedAmountByPeriod;

/*
Returns the total expense amount for the specified period by the specified interval
Days, Weeks, Months, Years
*/

public record GetGroupedAmountByPeriodQuery(
    DateOnly? FromDate,
    DateOnly? ToDate,
    ReportGroupingType GroupingType,
    Currency Currency
) : IRequest<GetGroupedAmountByPeriodResultDto>;
