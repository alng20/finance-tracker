using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.ItemCategories.DTOs;
using FinanceTracker.Domain.Entities;
using MediatR;

namespace FinanceTracker.Application.ItemCategories.Commands.UpdateItemCategory;

public class UpdateItemCategoryHandler(
    IItemCategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateItemCategoryCommand, ItemCategoryResultDto>
{
    private readonly IItemCategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ItemCategoryResultDto> Handle(
        UpdateItemCategoryCommand cmd,
        CancellationToken cancellationToken
    )
    {
        ItemCategory category = await _categoryRepository.GetByIdAsync(cmd.Id, cancellationToken);
        if (category.Name == cmd.Name)
        {
            return new ItemCategoryResultDto(category.Id, category.Name);
        }

        ItemCategory? categoryWithSameName = await _categoryRepository.FindByNameAsync(
            cmd.Name,
            cancellationToken
        );
        if (categoryWithSameName != null && categoryWithSameName.Id != category.Id)
        {
            throw new ConflictException(
                $"Item category with name '{categoryWithSameName.Name}' already exists"
            );
        }

        category.ChangeName(cmd.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ItemCategoryResultDto(category.Id, category.Name);
    }
}
