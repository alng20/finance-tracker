using FinanceTracker.Application.Items.DTOs;

using MediatR;

namespace FinanceTracker.Application.Items.Queries.SearchItem;

public record SearchItemQuery(string SearchString) : IRequest<IReadOnlyList<SearchItemResultDto>>;
