using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Infrastructure.Persistence;

namespace FinanceTracker.Infrastructure;

public class UnitOfWork(FinanceTrackerDbContext ctx) : IUnitOfWork
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _ctx.SaveChangesAsync(cancellationToken);
    }
}
