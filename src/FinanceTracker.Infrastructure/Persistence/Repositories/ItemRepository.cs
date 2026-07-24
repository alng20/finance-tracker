using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class ItemRepository(FinanceTrackerDbContext ctx) : IItemRepository
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<Item> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Item? item = await _ctx.Items.FindAsync(new object[] { id }, cancellationToken);
        return item ?? throw new ItemNotFoundException(id);
    }

    public void Add(Item item)
    {
        _ctx.Items.Add(item);
    }


    public async Task<IReadOnlyList<Item>> SearchAsync(
        string text,
        CancellationToken cancellationToken
    )
    {
        return await _ctx
            .Items.Where(item => item.Name.Contains(text))
            .ToListAsync(cancellationToken);
    }
}
