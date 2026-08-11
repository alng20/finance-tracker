using FinanceTracker.Application.Retailers.DTOs;

using MediatR;

namespace FinanceTracker.Application.Retailers.Commands.UpdateRetailer;

public record UpdateRetailerCommand(Guid Id, string Name) : IRequest<RetailerDto>;
