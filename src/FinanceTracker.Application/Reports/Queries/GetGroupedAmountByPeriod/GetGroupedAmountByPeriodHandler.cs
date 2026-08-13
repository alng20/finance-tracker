using System.Globalization;
using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Reports.DTOs;
using FinanceTracker.Application.Reports.Enums;
using FinanceTracker.Application.Reports.Models;
using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetGroupedAmountByPeriod;

public class GetGroupedAmountByPeriodHandler(
    ICurrentUserService currentUser,
    ICurrencyConverter currencyConverter,
    IExpenseReportRepository expenseReportRepository
) : IRequestHandler<GetGroupedAmountByPeriodQuery, GetGroupedAmountByPeriodResultDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ICurrencyConverter _currencyConverter = currencyConverter;
    private readonly IExpenseReportRepository _expenseReportRepository = expenseReportRepository;

    public async Task<GetGroupedAmountByPeriodResultDto> Handle(
        GetGroupedAmountByPeriodQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        if (
            query.GroupingType
            is ReportGroupingType.Day
                or ReportGroupingType.Month
                or ReportGroupingType.Year
        )
        {
            var groupedCurrencyAmount =
                await _expenseReportRepository.GetGroupedAmountByPeriodAsync(
                    userId,
                    query.FromDate,
                    query.ToDate,
                    query.GroupingType,
                    cancellationToken
                );

            var groupedAmount = groupedCurrencyAmount
                .GroupBy(x => x.ReportPeriod)
                .Select(g => new PeriodAmountReportDto(
                    g.Key,
                    g.Sum(x => _currencyConverter.Convert(x.Amount, x.Currency, query.Currency))
                ))
                .ToList();
            return new GetGroupedAmountByPeriodResultDto(query.GroupingType, groupedAmount);
        }
        else if (query.GroupingType is ReportGroupingType.Week)
        {
            var amounts = await _expenseReportRepository.GetExpensesAmountByDateAsync(
                userId,
                query.FromDate,
                query.ToDate,
                cancellationToken
            );
            var groupedAmount = amounts
                .GroupBy(x => new
                {
                    Year = x.Date.Year,
                    Week = ISOWeek.GetWeekOfYear(x.Date),
                    Currency = x.Currency,
                })
                .Select(g => new PeriodAmountReportDto(
                    ReportPeriod.GetWeekPeriod(g.Key.Year, g.Key.Week),
                    g.Sum(x => _currencyConverter.Convert(x.Amount, x.Currency, query.Currency))
                ))
                .ToList();
            return new GetGroupedAmountByPeriodResultDto(query.GroupingType, groupedAmount);
        }

        throw new NotFoundException("Report grouping type is unsupported");
    }
}
