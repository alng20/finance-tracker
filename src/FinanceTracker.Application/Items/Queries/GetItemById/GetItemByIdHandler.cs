using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Items.DTOs;

using MediatR;

namespace FinanceTracker.Application.Items.Queries.GetItemById;

public class GetItemHandler(IItemRepository itemRepository)
    : IRequestHandler<GetItemByIdQuery, ItemDto>
{
    private readonly IItemRepository _itemRepository = itemRepository;

    public async Task<ItemDto> Handle(
        GetItemByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var item = await _itemRepository.GetByIdAsync(query.Id, cancellationToken);
        return new ItemDto(item.Id, item.Name, item.CategoryId, item.Unit);
    }
}
