using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Retailers.DTOs;

using MediatR;

namespace FinanceTracker.Application.Retailers.Queries.GetRetailerById;

public class GetRetailerHandler(IRetailerRepository retailerRepository)
    : IRequestHandler<GetRetailerByIdQuery, RetailerDto>
{
    private readonly IRetailerRepository _retailerRepository = retailerRepository;

    public async Task<RetailerDto> Handle(
        GetRetailerByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var retailer = await _retailerRepository.GetByIdAsync(query.Id, cancellationToken);
        return new RetailerDto(retailer.Id, retailer.Name);
    }
}
