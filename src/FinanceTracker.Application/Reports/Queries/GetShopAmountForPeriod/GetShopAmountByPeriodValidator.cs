using FinanceTracker.Application.Reports.Queries.GetShopAmountForPeriod;

using FluentValidation;

namespace FinanceTracker.Application.Expenses.Commands.CreateExpense;

public class GetShopAmountForPeriodValidator : AbstractValidator<GetShopAmountForPeriodQuery>
{
    public GetShopAmountForPeriodValidator()
    {
        RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
