using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IRetailerRepository
{
    Task<Retailer> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Retailer>> GetAllAsync(CancellationToken cancellationToken);
    Task<Retailer?> FindByNameAsync(string name, CancellationToken cancellationToken);
    void Add(Retailer Retailer);
    void Delete(Retailer Retailer);
}
