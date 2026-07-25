using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IItemCategoryRepository
{
    Task<ItemCategory> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ItemCategory>> GetAllAsync(CancellationToken cancellationToken);
    Task<ItemCategory?> FindByNameAsync(string name, CancellationToken cancellationToken);
    void Add(ItemCategory category);
    void Delete(ItemCategory category);
}
