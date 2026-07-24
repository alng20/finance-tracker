using FinanceTracker.Application.ItemCategories.DTOs;

using MediatR;

namespace FinanceTracker.Application.ItemCategories.Queries.GetItemCategories;

public record GetItemCategoriesQuery : IRequest<IReadOnlyList<ItemCategoryResultDto>>;
