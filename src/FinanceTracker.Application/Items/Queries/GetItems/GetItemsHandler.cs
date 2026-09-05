using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Items.DTOs;
using MediatR;

namespace FinanceTracker.Application.Items.Queries.GetItems;

public class GetItemsHandler(IItemRepository categoryRepository)
    : IRequestHandler<GetItemsQuery, IReadOnlyList<ItemDto>>
{
    private readonly IItemRepository _ItemRepository = categoryRepository;

    public async Task<IReadOnlyList<ItemDto>> Handle(
        GetItemsQuery query,
        CancellationToken cancellationToken
    )
    {
        var Items = await _ItemRepository.GetAllAsync(query?.Count, cancellationToken);
        return Items.Select(x => new ItemDto(x.Id, x.Name, x.CategoryId, x.Unit)).ToList();
    }
}
