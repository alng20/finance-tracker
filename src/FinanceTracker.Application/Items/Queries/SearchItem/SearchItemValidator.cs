using FluentValidation;

namespace FinanceTracker.Application.Items.Queries.SearchItem;

public class CreateItemValidator : AbstractValidator<SearchItemQuery>
{
    public CreateItemValidator()
    {
        RuleFor(x => x.SearchString).NotEmpty().MinimumLength(2);
    }
}
