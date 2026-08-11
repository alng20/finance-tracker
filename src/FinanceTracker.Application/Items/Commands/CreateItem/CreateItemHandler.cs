using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Items.Commands.CreateItem;
using FinanceTracker.Application.Items.DTOs;
using FinanceTracker.Domain.Entities;

using MediatR;

namespace FinanceTracker.Application.Items.Commands.CreateItem;

public class CreateItemHandler(
    IItemRepository itemRepository,
    IItemCategoryRepository itemCategoryRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateItemCommand, ItemDto>
{
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IItemCategoryRepository _itemCategoryRepository = itemCategoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ItemDto> Handle(CreateItemCommand cmd, CancellationToken cancellationToken)
    {
        var itemWithName = await _itemRepository.FindByNameAsync(cmd.Name, cancellationToken);
        if (itemWithName != null)
        {
            throw new ConflictException($"Item with name '{itemWithName.Name}' already exists");
        }

        ItemCategory category = await _itemCategoryRepository.GetByIdAsync(
            cmd.CategoryId,
            cancellationToken
        );

        Item item = Item.Create(cmd.Name, category.Id, cmd.Unit);

        _itemRepository.Add(item);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ItemDto(item.Id, item.Name, item.CategoryId, item.Unit);
    }
}
