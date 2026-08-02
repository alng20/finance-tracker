using FinanceTracker.Application.Retailers.DTOs;

using MediatR;

namespace FinanceTracker.Application.Retailers.Queries.GetRetailerById;

public record GetRetailerByIdQuery(Guid Id) : IRequest<RetailerDto>;
