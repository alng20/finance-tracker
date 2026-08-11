using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IShopRepository
{
    Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken);
    Task<Shop> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Shop> GetByIdWithRetailerAsync(Guid id, CancellationToken cancellationToken);
    Task<Shop?> FindByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Shop>> SearchByNameAsync(string SearchString, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    void Add(Shop shop);
    void Delete(Shop shop);
}
