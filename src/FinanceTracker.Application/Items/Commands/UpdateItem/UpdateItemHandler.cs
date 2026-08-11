using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Items.DTOs;
using FinanceTracker.Domain.Entities;
using MediatR;

namespace FinanceTracker.Application.Items.Commands.UpdateItem;

public class UpdateItemHandler(
    IItemRepository itemRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateItemCommand, ItemDto>
{
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ItemDto> Handle(
        UpdateItemCommand cmd,
        CancellationToken cancellationToken
    )
    {
        Item item = await _itemRepository.GetByIdAsync(cmd.Id, cancellationToken);
        if (item.Name == cmd.Name)
        {
            return new ItemDto(item.Id, item.Name, item.CategoryId, item.Unit);
        }

        Item? itemWithSameName = await _itemRepository.FindByNameAsync(
            cmd.Name,
            cancellationToken
        );
        if (itemWithSameName != null && itemWithSameName.Id != item.Id)
        {
            throw new ConflictException(
                $"Item  with name '{itemWithSameName.Name}' already exists"
            );
        }

        item.Update(cmd.Name, cmd.CategoryId, cmd.Unit);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ItemDto(item.Id, item.Name, item.CategoryId, item.Unit);
    }
}
