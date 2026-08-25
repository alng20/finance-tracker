using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class ItemRepository(FinanceTrackerDbContext ctx) : IItemRepository
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _ctx.Items.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Item> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Item? item = await _ctx.Items.FindAsync(new object[] { id }, cancellationToken);
        return item ?? throw new NotFoundException($"Item with id '{id}' was not found");
    }

    public async Task<Item?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _ctx.Items.FirstOrDefaultAsync(
            x => EF.Functions.ILike(x.Name, name),
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<Item>> SearchByNameAsync(
        string SearchString,
        CancellationToken cancellationToken
    )
    {
        return await _ctx
            .Items.AsNoTracking()
            .Include(x => x.Category)
            .Where(x => EF.Functions.ILike(x.Name, $"%{SearchString}%"))
            .OrderBy(x => x.Name)
            .Take(10)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _ctx.Items.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(Item item)
    {
        _ctx.Items.Add(item);
    }

    public void Delete(Item item)
    {
        _ctx.Items.Remove(item);
    }
}
