using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Reports.DTOs;

using MediatR;

namespace FinanceTracker.Application.Reports.Queries.GetTotalAmountForPeriod;

public class GetTotalAmountForPeriodHandler(
    ICurrentUserService currentUser,
    ICurrencyConverter currencyConverter,
    IExpenseReportRepository expenseReportRepository
) : IRequestHandler<GetTotalAmountForPeriodQuery, GetTotalAmountForPeriodResultDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ICurrencyConverter _currencyConverter = currencyConverter;
    private readonly IExpenseReportRepository _expenseReportRepository = expenseReportRepository;

    public async Task<GetTotalAmountForPeriodResultDto> Handle(
        GetTotalAmountForPeriodQuery query,
        CancellationToken cancellationToken
    )
    {
        Guid userId = _currentUser.UserId;

        var amounts = await _expenseReportRepository.GetTotalAmountAsync(
            userId,
            query.FromDate,
            query.ToDate,
            cancellationToken
        );

        var total = amounts.Sum(x =>
            _currencyConverter.Convert(x.Amount, x.Currency, query.Currency)
        );

        return new GetTotalAmountForPeriodResultDto(
            query.FromDate,
            query.ToDate,
            total,
            query.Currency
        );
    }
}
