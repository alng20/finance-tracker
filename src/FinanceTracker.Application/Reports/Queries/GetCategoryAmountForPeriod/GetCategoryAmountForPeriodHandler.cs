using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Reports.DTOs;

using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetCategoryAmountForPeriod;

public class GetCategoryAmountForPeriodHandler(
    ICurrentUserService currentUser,
    ICurrencyConverter currencyConverter,
    IExpenseReportRepository expenseReportRepository
) : IRequestHandler<GetCategoryAmountForPeriodQuery, GetCategoryAmountForPeriodResultDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ICurrencyConverter _currencyConverter = currencyConverter;
    private readonly IExpenseReportRepository _expenseReportRepository = expenseReportRepository;

    public async Task<GetCategoryAmountForPeriodResultDto> Handle(
        GetCategoryAmountForPeriodQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var expenses = await _expenseReportRepository.GetExpensesTotalWithDetailsAsync(
            userId,
            query.FromDate,
            query.ToDate,
            cancellationToken
        );

        var details = expenses.SelectMany(expense => expense.Details).ToList();

        var totalAmount = expenses.Sum(x =>
            _currencyConverter.Convert(x.TotalAmount, x.Currency, query.Currency)
        );

        var detailedAmount = details.Sum(d =>
            _currencyConverter.Convert(d.TotalPrice, d.Currency, query.Currency)
        );
        var undetailedAmount = totalAmount - detailedAmount;

        var amounts = details
            .GroupBy(d => new { d.CategoryId, d.CategoryName })
            .Select(g => new CategoryAmountDto(
                g.Key.CategoryId,
                g.Key.CategoryName,
                g.Sum(x => _currencyConverter.Convert(x.TotalPrice, x.Currency, query.Currency))
            ))
            .OrderByDescending(x => x.TotalAmount)
            .ToList();

        return new GetCategoryAmountForPeriodResultDto(
            query.FromDate,
            query.ToDate,
            totalAmount,
            amounts,
            detailedAmount,
            undetailedAmount
        );
    }
}
