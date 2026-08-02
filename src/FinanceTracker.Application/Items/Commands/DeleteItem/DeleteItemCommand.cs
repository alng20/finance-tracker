using MediatR;

namespace FinanceTracker.Application.Items.Commands.DeleteItem;

public record DeleteItemCommand(Guid Id) : IRequest;
