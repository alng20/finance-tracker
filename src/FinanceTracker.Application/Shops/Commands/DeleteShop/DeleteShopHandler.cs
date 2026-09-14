using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;

using MediatR;

namespace FinanceTracker.Application.Shops.Commands.DeleteShop;

public class DeleteShopHandler(IShopRepository shopRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteShopCommand>
{
    private readonly IShopRepository _shopRepository = shopRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteShopCommand cmd, CancellationToken cancellationToken)
    {
        Shop shop = await _shopRepository.GetByIdAsync(cmd.Id, cancellationToken);
        shop.SoftDelete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
