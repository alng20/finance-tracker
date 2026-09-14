using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IItemRepository
{
    Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken);
    Task<Item> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(Item item);
    Task<Item?> FindByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Item>> SearchByNameAsync(
        string SearchString,
        CancellationToken cancellationToken
    );
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
