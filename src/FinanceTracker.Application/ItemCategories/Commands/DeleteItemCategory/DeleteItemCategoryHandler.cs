using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;
using MediatR;

namespace FinanceTracker.Application.ItemCategories.Commands.DeleteItemCategory;

public class DeleteItemCategoryHandler(
    IItemCategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteItemCategoryCommand>
{
    private readonly IItemCategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(
        DeleteItemCategoryCommand cmd,
        CancellationToken cancellationToken
    )
    {
        ItemCategory category = await _categoryRepository.GetByIdAsync(cmd.Id, cancellationToken);
        _categoryRepository.Delete(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
