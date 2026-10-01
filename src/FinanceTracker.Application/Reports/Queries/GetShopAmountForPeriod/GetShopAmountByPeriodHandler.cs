using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Reports.DTOs;

using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetShopAmountForPeriod;

public class GetShopAmountForPeriodHandler(
    ICurrentUserService currentUser,
    ICurrencyConverter currencyConverter,
    IExpenseReportRepository expenseReportRepository
) : IRequestHandler<GetShopAmountForPeriodQuery, GetShopAmountForPeriodResultDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ICurrencyConverter _currencyConverter = currencyConverter;
    private readonly IExpenseReportRepository _expenseReportRepository = expenseReportRepository;

    public async Task<GetShopAmountForPeriodResultDto> Handle(
        GetShopAmountForPeriodQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var expenses = await _expenseReportRepository.GetExpensesAmountByShopAsync(
            userId,
            query.FromDate,
            query.ToDate,
            cancellationToken
        );

        var amounts = expenses
            .GroupBy(x => new { x.ShopId, x.ShopName })
            .Select(x => new ShopAmountDto(
                x.Key.ShopId,
                x.Key.ShopName,
                x.Sum(x => _currencyConverter.Convert(x.TotalAmount, x.Currency, query.Currency))
            ))
            .OrderByDescending(x => x.TotalAmount)
            .ToList();

        var totalAmount = amounts.Sum(x => x.TotalAmount);

        return new GetShopAmountForPeriodResultDto(
            query.FromDate,
            query.ToDate,
            totalAmount,
            amounts
        );
    }
}
