using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.ItemCategories.DTOs;

using MediatR;

namespace FinanceTracker.Application.ItemCategories.Queries.GetItemCategory;

public class GetItemCategoryHandler(IItemCategoryRepository categoryRepository)
    : IRequestHandler<GetItemCategoryQuery, ItemCategoryResultDto>
{
    private readonly IItemCategoryRepository _categoryRepository = categoryRepository;

    public async Task<ItemCategoryResultDto> Handle(
        GetItemCategoryQuery query,
        CancellationToken cancellationToken
    )
    {
        var category = await _categoryRepository.GetByIdAsync(query.Id, cancellationToken);
        return new ItemCategoryResultDto(category.Id, category.Name);
    }
}
