using FinanceTracker.Application.Items.DTOs;

using MediatR;

namespace FinanceTracker.Application.Items.Queries.GetItems;

public record GetItemsQuery(int? Count) : IRequest<IReadOnlyList<ItemDto>>;
