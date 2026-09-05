namespace FinanceTracker.Api.Requests;

public sealed class GetExpensesRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;

    public DateOnly? FromDate { get; init; }
    public DateOnly? ToDate { get; init; }

    public IReadOnlyCollection<Guid>? ShopIds { get; init; }
    public IReadOnlyCollection<Guid>? CategoryIds { get; init; }
    public IReadOnlyCollection<Guid>? ItemIds { get; init; }
    public IReadOnlyCollection<Guid>? RetailerIds { get; init; }

    public decimal? PriceFrom { get; init; }
    public decimal? PriceTo { get; init; }
}
