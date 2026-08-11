using FluentValidation;

namespace FinanceTracker.Application.Expenses.Commands.GetExpensesByUser;

public class GetExpensesByUserValidator : AbstractValidator<GetExpensesByUserQuery>
{
    public GetExpensesByUserValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 500);
        RuleFor(x => x)
            .Must(x => !x.FromDate.HasValue || !x.ToDate.HasValue || x.FromDate <= x.ToDate)
            .WithMessage("FromDate must be less than or equal to ToDate.");
    }
}
