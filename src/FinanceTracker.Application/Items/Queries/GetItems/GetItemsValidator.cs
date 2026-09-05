using FluentValidation;

namespace FinanceTracker.Application.Items.Queries.GetItems;

public class GetItemsValidator : AbstractValidator<GetItemsQuery>
{
    public GetItemsValidator()
    {
        RuleFor(x => x.Count).GreaterThan(0).When(x => x.Count.HasValue);
    }
}
