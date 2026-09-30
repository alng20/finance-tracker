namespace FinanceTracker.Application.Common.Models;

public readonly record struct None;

public record PagedResultWithMetadata<DataType, MetadataType>(IReadOnlyList<DataType> Data, MetadataType Metadata, int Page, int PageSize, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}

public record PagedResult<DataType>(IReadOnlyList<DataType> Data, int Page, int PageSize, int TotalCount) : PagedResultWithMetadata<DataType, None>(Data, default, Page, PageSize, TotalCount);

