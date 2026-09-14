using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;

using MediatR;

namespace FinanceTracker.Application.Items.Commands.DeleteItem;

public class DeleteItemHandler(IItemRepository itemRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteItemCommand>
{
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteItemCommand cmd, CancellationToken cancellationToken)
    {
        Item item = await _itemRepository.GetByIdAsync(cmd.Id, cancellationToken);
        item.SoftDelete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
