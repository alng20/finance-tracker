using FinanceTracker.Application.Items.DTOs;
using MediatR;

namespace FinanceTracker.Application.Items.Commands.UpdateItem;

public record UpdateItemCommand(Guid Id, string Name, Guid CategoryId, Domain.Enums.Unit Unit)
    : IRequest<ItemDto>;
