using FluentValidation;

namespace FinanceTracker.Application.Reports.Queries.GetTotalAmountForPeriod;

public class GetTotalAmountForPeriodValidator : AbstractValidator<GetTotalAmountForPeriodQuery>
{
    public GetTotalAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
