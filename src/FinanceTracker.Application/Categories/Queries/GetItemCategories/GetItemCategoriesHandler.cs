using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.ItemCategories.DTOs;

using MediatR;

namespace FinanceTracker.Application.ItemCategories.Queries.GetItemCategories;

public class GetItemCategoriesHandler(IItemCategoryRepository categoryRepository)
    : IRequestHandler<GetItemCategoriesQuery, IReadOnlyList<ItemCategoryResultDto>>
{
    private readonly IItemCategoryRepository _categoryRepository = categoryRepository;

    public async Task<IReadOnlyList<ItemCategoryResultDto>> Handle(
        GetItemCategoriesQuery query,
        CancellationToken cancellationToken
    )
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(x => new ItemCategoryResultDto(x.Id, x.Name)).ToList();
    }
}
