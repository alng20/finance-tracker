using FluentValidation;

namespace FinanceTracker.Application.Reports.Queries.GetRetailerAmountForPeriod;

public class GetRetailerAmountForPeriodValidator
    : AbstractValidator<GetRetailerAmountForPeriodQuery>
{
    public GetRetailerAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate)
            .GreaterThan(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        ;
        RuleFor(x => x.Currency).IsInEnum();
    }
}
