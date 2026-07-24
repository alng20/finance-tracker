using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IItemRepository
{
    Task<Item> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    void Add(Item item);

    Task<IReadOnlyList<Item>> SearchAsync(string text, CancellationToken cancellationToken);
}
