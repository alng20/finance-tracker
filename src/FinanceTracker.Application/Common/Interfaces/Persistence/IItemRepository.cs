using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IItemRepository
{
    Task<Item> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    void Add(Item item);

    Task<Item?> FindByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Item>> SearchByNameAsync(
        string SearchString,
        CancellationToken cancellationToken
    );
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
