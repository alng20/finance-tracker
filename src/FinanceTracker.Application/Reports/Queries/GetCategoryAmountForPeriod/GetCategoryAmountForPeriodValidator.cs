using FluentValidation;

namespace FinanceTracker.Application.Reports.Queries.GetCategoryAmountForPeriod;

public class GetCategoryAmountForPeriodValidator
    : AbstractValidator<GetCategoryAmountForPeriodQuery>
{
    public GetCategoryAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate)
            .GreaterThan(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        ;
        RuleFor(x => x.Currency).IsInEnum();
    }
}
