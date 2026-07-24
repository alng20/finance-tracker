using FinanceTracker.Application.Items.DTOs;

using MediatR;

namespace FinanceTracker.Application.Items.Commands.CreateItem;

public record CreateItemCommand(
    string Name,
    Guid CategoryId,
    Domain.Enums.Unit Unit
) : IRequest<ItemDto>;
