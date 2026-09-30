using FluentValidation;

namespace FinanceTracker.Application.Reports.Queries.GetTotalAmountForPeriod;

public class GetTotalAmountForPeriodValidator : AbstractValidator<GetTotalAmountForPeriodQuery>
{
    public GetTotalAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate)
            .GreaterThan(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        ;
        RuleFor(x => x.Currency).IsInEnum();
    }
}
