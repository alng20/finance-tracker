using FinanceTracker.Application.Reports.Queries.GetRetailerAmountForPeriod;

using FluentValidation;

namespace FinanceTracker.Application.Expenses.Commands.CreateExpense;

public class GetRetailerAmountForPeriodValidator : AbstractValidator<GetRetailerAmountForPeriodQuery>
{
    public GetRetailerAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
