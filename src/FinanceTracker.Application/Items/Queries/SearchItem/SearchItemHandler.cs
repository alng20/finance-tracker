using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Items.DTOs;
using MediatR;

namespace FinanceTracker.Application.Items.Queries.SearchItem;

public class SearchItemHandler(IItemRepository itemRepository)
    : IRequestHandler<SearchItemQuery, IReadOnlyList<SearchItemResultDto>>
{
    private readonly IItemRepository _itemRepository = itemRepository;

    public async Task<IReadOnlyList<SearchItemResultDto>> Handle(
        SearchItemQuery query,
        CancellationToken cancellationToken
    )
    {
        var items = await _itemRepository.SearchByNameAsync(query.SearchString, cancellationToken);
        return items.Select(x => new SearchItemResultDto(x.Id, x.Name, x.Unit)).ToList();
    }
}
