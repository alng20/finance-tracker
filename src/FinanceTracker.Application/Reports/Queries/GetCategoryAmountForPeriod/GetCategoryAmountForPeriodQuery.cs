using FinanceTracker.Application.Reports.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetCategoryAmountForPeriod;

/*
Returns the total expense amount for the specified period,
the amount covered by expense details,
and the detailed amount grouped by category.
*/

public record GetCategoryAmountForPeriodQuery(
    DateOnly? FromDate,
    DateOnly? ToDate,
    Currency Currency
) : IRequest<GetCategoryAmountForPeriodResultDto>;
