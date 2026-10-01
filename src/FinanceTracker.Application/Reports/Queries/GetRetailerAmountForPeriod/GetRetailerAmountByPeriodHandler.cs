using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Reports.DTOs;

using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetRetailerAmountForPeriod;

public class GetRetailerAmountForPeriodHandler(
    ICurrentUserService currentUser,
    ICurrencyConverter currencyConverter,
    IExpenseReportRepository expenseReportRepository
) : IRequestHandler<GetRetailerAmountForPeriodQuery, GetRetailerAmountForPeriodResultDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ICurrencyConverter _currencyConverter = currencyConverter;
    private readonly IExpenseReportRepository _expenseReportRepository = expenseReportRepository;

    public async Task<GetRetailerAmountForPeriodResultDto> Handle(
        GetRetailerAmountForPeriodQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var expenses = await _expenseReportRepository.GetExpensesAmountByRetailerAsync(
            userId,
            query.FromDate,
            query.ToDate,
            cancellationToken
        );

        var amounts = expenses
            .GroupBy(x => new { x.RetailerId, x.RetailerName })
            .Select(x => new RetailerAmountDto(
                x.Key.RetailerId,
                x.Key.RetailerName,
                x.Sum(x => _currencyConverter.Convert(x.TotalAmount, x.Currency, query.Currency))
            ))
            .OrderByDescending(x => x.TotalAmount)
            .ToList();

        var totalAmount = amounts.Sum(x => x.TotalAmount);

        return new GetRetailerAmountForPeriodResultDto(
            query.FromDate,
            query.ToDate,
            totalAmount,
            amounts
        );
    }
}
