using FinanceTracker.Application.Items.DTOs;

using MediatR;

namespace FinanceTracker.Application.Items.Queries.SearchItems;

public record SearchItemsQuery(string Text) : IRequest<IReadOnlyList<SearchItemsResultDto>>;
