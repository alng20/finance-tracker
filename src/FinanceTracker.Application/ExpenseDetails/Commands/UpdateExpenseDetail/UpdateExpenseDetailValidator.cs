using FluentValidation;

namespace FinanceTracker.Application.ExpenseDetails.Commands.UpdateExpenseDetail;

public class UpdateExpenseDetailValidator : AbstractValidator<UpdateExpenseDetailCommand>
{
    public UpdateExpenseDetailValidator()
    {
        RuleFor(x => x.TotalPrice).GreaterThan(0);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0, 100);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
