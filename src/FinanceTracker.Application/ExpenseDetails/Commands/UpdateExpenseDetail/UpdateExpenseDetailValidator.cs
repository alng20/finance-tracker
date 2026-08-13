using FinanceTracker.Application.Common.Enums;

using FluentValidation;

namespace FinanceTracker.Application.ExpenseDetails.Commands.UpdateExpenseDetail;

public class UpdateExpenseDetailValidator : AbstractValidator<UpdateExpenseDetailCommand>
{
    public UpdateExpenseDetailValidator()
    {
        RuleFor(x => x.TotalPrice).GreaterThan(0);
        RuleFor(x => x.Discount!.Value)
            .InclusiveBetween(0, 100)
            .When(x => x.Discount?.Type == DiscountType.Percent);
        RuleFor(x => x.Discount!.Value)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Discount?.Type == DiscountType.Amount);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
