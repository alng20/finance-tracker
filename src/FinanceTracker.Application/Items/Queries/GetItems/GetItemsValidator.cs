using FluentValidation;

namespace FinanceTracker.Application.Items.Queries.GetItems;

public class GetItemsValidator : AbstractValidator<GetItemsQuery>
{
    public GetItemsValidator()
    {
        // TODO: Validate item filters
    }
}
