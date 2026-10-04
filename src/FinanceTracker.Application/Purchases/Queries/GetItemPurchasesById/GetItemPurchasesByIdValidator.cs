using FinanceTracker.Application.Common.Consts;

using FluentValidation;

namespace FinanceTracker.Application.Purchases.Queries.GetItemPurchasesById;

public class GetItemPurchasesByIdValidator : AbstractValidator<GetItemPurchasesByIdQuery>
{
    public GetItemPurchasesByIdValidator()
    {
        RuleFor(x => x.ToDate)
            .GreaterThan(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        ;
        RuleFor(x => x.Currency).IsInEnum();
        RuleFor(x => x.SortType).IsInEnum();
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, PageConsts.MaxPageSize);
    }
}
