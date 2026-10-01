using FluentValidation;

namespace FinanceTracker.Application.Reports.Queries.GetShopAmountForPeriod;

public class GetShopAmountForPeriodValidator : AbstractValidator<GetShopAmountForPeriodQuery>
{
    public GetShopAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate)
            .GreaterThan(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        ;
        RuleFor(x => x.Currency).IsInEnum();
    }
}
