using FinanceTracker.Application.Reports.DTOs;
using FinanceTracker.Domain.Enums;

using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetShopAmountForPeriod;

/*
Returns the total expense amount for the specified shop and period
*/

public record GetShopAmountForPeriodQuery(
    DateOnly? FromDate,
    DateOnly? ToDate,
    Currency Currency
) : IRequest<GetShopAmountForPeriodResultDto>;
