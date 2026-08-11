using FluentValidation;

namespace FinanceTracker.Application.ExpenseDetails.Commands.CreateExpenseDetail;

public class CreateExpenseDetailValidator : AbstractValidator<CreateExpenseDetailCommand>
{
    public CreateExpenseDetailValidator()
    {
        RuleFor(x => x.TotalPrice).GreaterThan(0);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0, 100);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
