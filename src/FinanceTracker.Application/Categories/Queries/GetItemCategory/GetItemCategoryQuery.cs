using FinanceTracker.Application.ItemCategories.DTOs;

using MediatR;

namespace FinanceTracker.Application.ItemCategories.Queries.GetItemCategory;

public record GetItemCategoryQuery(Guid Id) : IRequest<ItemCategoryResultDto>;
