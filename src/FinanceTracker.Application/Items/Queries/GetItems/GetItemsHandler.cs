using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Items.DTOs;

using MediatR;

namespace FinanceTracker.Application.Items.Queries.GetItems;

public class GetItemsHandler(IItemProvider itemProvider)
    : IRequestHandler<GetItemsQuery, PagedResult<GetItemsResultDto>>
{
    private readonly IItemProvider _itemProvider = itemProvider;

    public async Task<PagedResult<GetItemsResultDto>> Handle(
        GetItemsQuery query,
        CancellationToken cancellationToken
    )
    {
        return await _itemProvider.GetAsync(
            query.Page,
            query.PageSize,
            query.CategoryIds,
            query.Units,
            cancellationToken
        );
    }
}
