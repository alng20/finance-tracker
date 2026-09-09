using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Items.DTOs;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Items.Queries.GetItems;

public record GetItemsQuery(
    int Page,
    int PageSize,
    IReadOnlyCollection<Guid>? CategoryIds,
    IReadOnlyCollection<Unit>? Units
) : MediatR.IRequest<PagedResult<GetItemsResultDto>>;
