using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class ItemCategoryRepository(FinanceTrackerDbContext ctx) : IItemCategoryRepository
{
    private readonly FinanceTrackerDbContext _ctx = ctx;

    public async Task<ItemCategory> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ItemCategory? category = await _ctx.ItemCategories.FindAsync(new object[] { id }, cancellationToken);
        return category ?? throw new ItemCategoryNotFoundException(id);
    }

    public async Task<IReadOnlyList<ItemCategory>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _ctx.ItemCategories.ToListAsync(cancellationToken);
    }

    public void Add(ItemCategory category)
    {
        _ctx.ItemCategories.Add(category);
    }
}
