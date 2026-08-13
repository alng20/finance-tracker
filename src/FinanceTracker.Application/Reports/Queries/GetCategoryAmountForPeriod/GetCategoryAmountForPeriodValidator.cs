using FinanceTracker.Application.Reports.Queries.GetCategoryAmountForPeriod;

using FluentValidation;

namespace FinanceTracker.Application.Expenses.Commands.CreateExpense;

public class GetCategoryAmountForPeriodValidator : AbstractValidator<GetCategoryAmountForPeriodQuery>
{
    public GetCategoryAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
