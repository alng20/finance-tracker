using FluentValidation;

namespace FinanceTracker.Application.Items.Queries.GetItems;

public class GetItemsValidator : AbstractValidator<GetItemsQuery>
{
    public GetItemsValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 500);
    }
}
