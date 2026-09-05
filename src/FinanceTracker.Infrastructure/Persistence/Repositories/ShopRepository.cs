using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class ShopRepository(FinanceTrackerDbContext ctx) : IShopRepository
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _ctx.Shops.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Shop>> GetAllAsync(CancellationToken cancellationToken)
    {
        // TODO: Sort shop and add pagination
        return await _ctx.Shops.Include(x => x.Retailer).ToListAsync(cancellationToken);
    }

    public async Task<Shop> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Shop? shop = await _ctx.Shops.FindAsync(new object[] { id }, cancellationToken);
        return shop ?? throw new NotFoundException($"Shop with id {id} was not found");
    }

    public async Task<Shop> GetByIdWithRetailerAsync(Guid id, CancellationToken cancellationToken)
    {
        var shop = await _ctx
            .Shops.Include(x => x.Retailer)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return shop ?? throw new NotFoundException($"Shop with id {id} was not found");
    }

    public async Task<Shop?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _ctx.Shops.FirstOrDefaultAsync(
            x => EF.Functions.ILike(x.Name, name),
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<Shop>> SearchByNameAsync(
        string SearchString,
        CancellationToken cancellationToken
    )
    {
        return await _ctx
            .Shops.AsNoTracking()
            .Where(x => EF.Functions.ILike(x.Name, $"%{SearchString}%"))
            .OrderBy(x => x.Name)
            .Take(10)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _ctx.Shops.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(Shop shop)
    {
        _ctx.Shops.Add(shop);
    }

    public void Delete(Shop shop)
    {
        _ctx.Shops.Remove(shop);
    }
}
