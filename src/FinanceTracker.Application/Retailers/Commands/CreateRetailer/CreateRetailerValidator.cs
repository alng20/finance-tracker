using FluentValidation;

namespace FinanceTracker.Application.Retailers.Commands.CreateRetailer;

public class CreateRetailerValidator : AbstractValidator<CreateRetailerCommand>
{
    public CreateRetailerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
