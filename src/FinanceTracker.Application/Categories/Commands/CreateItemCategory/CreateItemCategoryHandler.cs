using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.ItemCategories.DTOs;
using FinanceTracker.Domain.Entities;

using MediatR;

namespace FinanceTracker.Application.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryHandler(
    IItemCategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateItemCategoryCommand, ItemCategoryResultDto>
{
    private readonly IItemCategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ItemCategoryResultDto> Handle(
        CreateItemCategoryCommand cmd,
        CancellationToken cancellationToken
    )
    {
        ItemCategory category = ItemCategory.Create(cmd.Name);
        _categoryRepository.Add(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ItemCategoryResultDto(category.Id, category.Name);
    }
}
