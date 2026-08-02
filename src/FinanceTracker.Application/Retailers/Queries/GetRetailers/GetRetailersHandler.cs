using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Retailers.DTOs;

using MediatR;

namespace FinanceTracker.Application.Retailers.Queries.GetRetailers;

public class GetRetailersHandler(IRetailerRepository retailerRepository)
    : IRequestHandler<GetRetailersQuery, IReadOnlyList<RetailerDto>>
{
    private readonly IRetailerRepository _retailerRepository = retailerRepository;

    public async Task<IReadOnlyList<RetailerDto>> Handle(
        GetRetailersQuery query,
        CancellationToken cancellationToken
    )
    {
        var retailers = await _retailerRepository.GetAllAsync(cancellationToken);
        return retailers.Select(x => new RetailerDto(x.Id, x.Name)).ToList();
    }
}
