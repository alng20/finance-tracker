using FinanceTracker.Application.Items.DTOs;

using MediatR;

namespace FinanceTracker.Application.Items.Queries.GetItemById;

public record GetItemByIdQuery(Guid Id) : IRequest<ItemDto>;
