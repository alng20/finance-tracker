using FluentValidation;

namespace FinanceTracker.Application.Expenses.Commands.UpdateExpense;

public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseValidator()
    {
        RuleFor(x => x.TotalAmount).GreaterThan(0);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
