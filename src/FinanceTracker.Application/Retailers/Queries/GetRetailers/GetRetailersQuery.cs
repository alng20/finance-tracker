using FinanceTracker.Application.Retailers.DTOs;

using MediatR;

namespace FinanceTracker.Application.Retailers.Queries.GetRetailers;

public record GetRetailersQuery : IRequest<IReadOnlyList<RetailerDto>>;
