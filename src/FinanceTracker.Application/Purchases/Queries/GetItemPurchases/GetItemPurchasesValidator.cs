using FinanceTracker.Application.Common.Consts;

using FluentValidation;

namespace FinanceTracker.Application.Purchases.Queries.GetItemPurchases;

public class GetItemPurchasesValidator : AbstractValidator<GetItemPurchasesQuery>
{
    public GetItemPurchasesValidator()
    {
        RuleFor(x => x.ToDate)
            .GreaterThan(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        ;
        RuleFor(x => x.Currency).IsInEnum();
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, PageConsts.MaxPageSize);
    }
}
