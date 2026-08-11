using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class RetailerRepository(FinanceTrackerDbContext ctx) : IRetailerRepository
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<Retailer> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Retailer? retailer = await _ctx.Retailers.FindAsync(new object[] { id }, cancellationToken);
        return retailer ?? throw new NotFoundException($"Retailer with id {id} was not found");
    }

    public async Task<IReadOnlyList<Retailer>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _ctx.Retailers.ToListAsync(cancellationToken);
    }

    public async Task<Retailer?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _ctx.Retailers.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public void Add(Retailer retailer)
    {
        _ctx.Retailers.Add(retailer);
    }

    public void Delete(Retailer retailer)
    {
        _ctx.Retailers.Remove(retailer);
    }
}
