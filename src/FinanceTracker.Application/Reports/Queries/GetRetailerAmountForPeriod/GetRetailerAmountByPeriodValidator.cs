using FluentValidation;

namespace FinanceTracker.Application.Reports.Queries.GetRetailerAmountForPeriod;

public class GetRetailerAmountForPeriodValidator
    : AbstractValidator<GetRetailerAmountForPeriodQuery>
{
    public GetRetailerAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
