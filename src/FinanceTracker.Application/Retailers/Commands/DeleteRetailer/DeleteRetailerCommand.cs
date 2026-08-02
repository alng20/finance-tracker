using MediatR;

namespace FinanceTracker.Application.Retailers.Commands.DeleteRetailer;

public record DeleteRetailerCommand(Guid Id) : IRequest;
