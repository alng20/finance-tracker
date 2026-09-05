using FluentValidation;

namespace FinanceTracker.Application.Expenses.Queries.GetExpensesByUser;

public class GetExpensesByUserValidator : AbstractValidator<GetExpensesByUserQuery>
{
    public GetExpensesByUserValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 500);
        RuleFor(x => x)
            .Must(x => !x.FromDate.HasValue || !x.ToDate.HasValue || x.FromDate <= x.ToDate)
            .WithMessage("Date range is invalid.");
        RuleFor(x => x)
            .Must(x => !x.FromAmount.HasValue || !x.ToAmount.HasValue || x.FromAmount <= x.ToAmount)
            .WithMessage("Amount period is invalid.");
        RuleFor(x => x.Currency).IsInEnum().When(x => x.Currency.HasValue);
    }
}
