using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IItemCategoryRepository
{
    Task<ItemCategory> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ItemCategory>> GetAllAsync(CancellationToken cancellationToken);
    void Add(ItemCategory category);
}
