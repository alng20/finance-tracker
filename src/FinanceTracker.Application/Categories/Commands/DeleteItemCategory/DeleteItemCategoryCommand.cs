using MediatR;

namespace FinanceTracker.Application.ItemCategories.Commands.DeleteItemCategory;

public record DeleteItemCategoryCommand(Guid Id) : IRequest;
