using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Items.DTOs;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Items.Queries.GetItems;

public record GetItemsQuery(
    int Page = 1,
    int PageSize = 20,
    IReadOnlyCollection<Guid>? CategoryIds = null,
    IReadOnlyCollection<Unit>? Units = null
) : MediatR.IRequest<PagedResult<GetItemsResultDto>>;
