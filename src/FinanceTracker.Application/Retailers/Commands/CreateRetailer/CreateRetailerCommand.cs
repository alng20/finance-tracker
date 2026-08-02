using FinanceTracker.Application.Retailers.DTOs;

using MediatR;

namespace FinanceTracker.Application.Retailers.Commands.CreateRetailer;

public record CreateRetailerCommand(string Name) : IRequest<RetailerDto>;
