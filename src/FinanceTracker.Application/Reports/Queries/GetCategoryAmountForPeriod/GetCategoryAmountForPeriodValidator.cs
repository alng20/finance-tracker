using FluentValidation;

namespace FinanceTracker.Application.Reports.Queries.GetCategoryAmountForPeriod;

public class GetCategoryAmountForPeriodValidator : AbstractValidator<GetCategoryAmountForPeriodQuery>
{
    public GetCategoryAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
