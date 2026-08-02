using FluentValidation;

namespace FinanceTracker.Application.Expenses.Commands.CreateExpense;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.TotalAmount).GreaterThan(0);
        RuleFor(x => x.Currency).IsInEnum();
        RuleForEach(x => x.Details)
            .ChildRules(detail =>
            {
                detail.RuleFor(d => d.TotalPrice).GreaterThanOrEqualTo(0);
                detail.RuleFor(d => d.DiscountPercent).InclusiveBetween(0, 100);
                detail.RuleFor(d => d.Quantity).GreaterThan(0);
            });
    }
}
