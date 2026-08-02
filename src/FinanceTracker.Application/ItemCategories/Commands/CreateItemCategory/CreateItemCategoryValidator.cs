using FluentValidation;

namespace FinanceTracker.Application.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryValidator : AbstractValidator<CreateItemCategoryCommand>
{
    public CreateItemCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
