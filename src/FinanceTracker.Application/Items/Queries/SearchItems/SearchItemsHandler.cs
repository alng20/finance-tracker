using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Items.DTOs;

using MediatR;

namespace FinanceTracker.Application.Items.Queries.SearchItems;

public class SearchItemsHandler(IItemRepository itemRepository)
    : IRequestHandler<SearchItemsQuery, IReadOnlyList<SearchItemsResultDto>>
{
    private readonly IItemRepository _itemRepository = itemRepository;

    public async Task<IReadOnlyList<SearchItemsResultDto>> Handle(
        SearchItemsQuery query,
        CancellationToken cancellationToken
    )
    {
        var items = await _itemRepository.SearchAsync(query.Text, cancellationToken);
        return items.Select(x => new SearchItemsResultDto(x.Id, x.Name, x.CategoryId, x.Unit)).ToList();
    }
}
