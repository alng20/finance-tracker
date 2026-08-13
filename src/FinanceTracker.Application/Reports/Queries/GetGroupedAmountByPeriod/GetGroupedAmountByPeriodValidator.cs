using FinanceTracker.Application.Reports.Queries.GetGroupedAmountByPeriod;

using FluentValidation;

namespace FinanceTracker.Application.Expenses.Commands.CreateExpense;

public class GetGroupedAmountByPeriodValidator : AbstractValidator<GetGroupedAmountByPeriodQuery>
{
    public GetGroupedAmountByPeriodValidator()
    {
        RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate);
        RuleFor(x => x.Currency).IsInEnum();
        RuleFor(x => x.GroupingType).IsInEnum();
    }
}
