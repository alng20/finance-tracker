using FinanceTracker.Application.Shops.Queries.SearchShop;

using FluentValidation;

namespace FinanceTracker.Application.Shops.Commands.SearchShop;

public class CreateShopValidator : AbstractValidator<SearchShopQuery>
{
    public CreateShopValidator()
    {
        RuleFor(x => x.SearchString).NotEmpty().MinimumLength(2);
    }
}
