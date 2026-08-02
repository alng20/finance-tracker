using FinanceTracker.Application.ItemCategories.DTOs;

using MediatR;

namespace FinanceTracker.Application.ItemCategories.Commands.CreateItemCategory;

public record CreateItemCategoryCommand(string Name) : IRequest<ItemCategoryDto>;
