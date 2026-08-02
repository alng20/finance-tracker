using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Retailers.DTOs;
using FinanceTracker.Domain.Entities;
using MediatR;

namespace FinanceTracker.Application.Retailers.Commands.UpdateRetailer;

public class UpdateRetailerHandler(
    IRetailerRepository retailerRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateRetailerCommand, RetailerDto>
{
    private readonly IRetailerRepository _retailerRepository = retailerRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RetailerDto> Handle(
        UpdateRetailerCommand cmd,
        CancellationToken cancellationToken
    )
    {
        Retailer retailer = await _retailerRepository.GetByIdAsync(cmd.Id, cancellationToken);
        if (retailer.Name == cmd.Name)
        {
            return new RetailerDto(retailer.Id, retailer.Name);
        }

        Retailer? retailerWithSameName = await _retailerRepository.FindByNameAsync(
            cmd.Name,
            cancellationToken
        );
        if (retailerWithSameName != null && retailerWithSameName.Id != retailer.Id)
        {
            throw new ConflictException(
                $"Retailer with name '{retailerWithSameName.Name}' already exists"
            );
        }

        retailer.ChangeName(cmd.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RetailerDto(retailer.Id, retailer.Name);
    }
}
