using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;
using MediatR;

namespace FinanceTracker.Application.Retailers.Commands.DeleteRetailer;

public class DeleteRetailerHandler(
    IRetailerRepository retailerRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteRetailerCommand>
{
    private readonly IRetailerRepository _retailerRepository = retailerRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(
        DeleteRetailerCommand cmd,
        CancellationToken cancellationToken
    )
    {
        Retailer retailer = await _retailerRepository.GetByIdAsync(cmd.Id, cancellationToken);
        _retailerRepository.Delete(retailer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
