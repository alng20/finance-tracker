using FluentValidation;

namespace FinanceTracker.Application.Shops.Queries.GetShops;

public class GetShopsValidator : AbstractValidator<GetShopsQuery>
{
    public GetShopsValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 500);
    }
}
