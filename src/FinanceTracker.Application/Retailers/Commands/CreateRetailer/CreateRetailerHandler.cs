using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Retailers.DTOs;
using FinanceTracker.Domain.Entities;
using MediatR;

namespace FinanceTracker.Application.Retailers.Commands.CreateRetailer;

public class CreateRetailerHandler(
    IRetailerRepository retailerRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateRetailerCommand, RetailerDto>
{
    private readonly IRetailerRepository _retailerRepository = retailerRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RetailerDto> Handle(
        CreateRetailerCommand cmd,
        CancellationToken cancellationToken
    )
    {
        Retailer? retailerWithName = await _retailerRepository.FindByNameAsync(
            cmd.Name,
            cancellationToken
        );
        if (retailerWithName != null)
        {
            throw new ConflictException(
                $"Retailer {retailerWithName.Id} with name '{retailerWithName.Name}' already exists"
            );
        }

        Retailer retailer = Retailer.Create(cmd.Name);
        _retailerRepository.Add(retailer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RetailerDto(retailer.Id, retailer.Name);
    }
}
