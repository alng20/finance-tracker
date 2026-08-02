using FluentValidation;

namespace FinanceTracker.Application.Items.Commands.CreateItem;

public class CreateItemValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Unit).IsInEnum();
    }
}
