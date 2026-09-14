namespace FinanceTracker.Application.Common.Models;

public record PagedResult<T>(IReadOnlyList<T> Data, int Page, int PageSize, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
