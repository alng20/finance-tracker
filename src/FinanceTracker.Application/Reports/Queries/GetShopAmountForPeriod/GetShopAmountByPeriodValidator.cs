using FluentValidation;

namespace FinanceTracker.Application.Reports.Queries.GetShopAmountForPeriod;

public class GetShopAmountForPeriodValidator : AbstractValidator<GetShopAmountForPeriodQuery>
{
    public GetShopAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
