using FinanceTracker.Application.Items.Queries.SearchItem;

using FluentValidation;

namespace FinanceTracker.Application.Items.Commands.SearchItem;

public class CreateItemValidator : AbstractValidator<SearchItemQuery>
{
    public CreateItemValidator()
    {
        RuleFor(x => x.SearchString).NotEmpty().MinimumLength(2);
    }
}
