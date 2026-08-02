using FinanceTracker.Application.ItemCategories.DTOs;

using MediatR;

namespace FinanceTracker.Application.ItemCategories.Commands.UpdateItemCategory;

public record UpdateItemCategoryCommand(Guid Id, string Name) : IRequest<ItemCategoryDto>;
