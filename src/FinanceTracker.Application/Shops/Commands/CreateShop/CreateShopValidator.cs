using FluentValidation;

namespace FinanceTracker.Application.Shops.Commands.CreateShop;

public class CreateShopValidator : AbstractValidator<CreateShopCommand>
{
    public CreateShopValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
