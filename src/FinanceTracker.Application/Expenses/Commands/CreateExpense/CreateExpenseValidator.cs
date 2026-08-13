using FinanceTracker.Application.Common.Enums;

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
                detail
                    .RuleFor(x => x.Discount!.Value)
                    .InclusiveBetween(0, 100)
                    .When(x => x.Discount?.Type == DiscountType.Percent);
                detail.RuleFor(x => x.Discount!.Value)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.Discount?.Type == DiscountType.Amount);
                detail.RuleFor(d => d.Quantity).GreaterThan(0);
            });
    }
}
