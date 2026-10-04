using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Item
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public ItemCategory Category { get; private set; } = null!;
    public Unit Unit { get; private set; }

    // TODO: Add price per 1 unit for piece items (100g, 100ml, 1piece, etc.) to track different sizes items

    public DateTimeOffset? DeletedAt { get; private set; }

    public static Item Create(string name, Guid categoryId, Unit unit)
    {
        return new Item(Guid.NewGuid(), name, categoryId, unit);
    }

    private Item(Guid id, string name, Guid categoryId, Unit unit)
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(name, nameof(name));

        Id = id;
        Name = name;
        CategoryId = categoryId;
        Unit = unit;
    }

    public void Update(string name, Guid categoryId, Unit unit)
    {
        Guard.AgainstEmpty(name, nameof(name));
        Guard.AgainstEmpty(categoryId, nameof(categoryId));

        Name = name;
        CategoryId = categoryId;
        Unit = unit;
    }

    public void ChangeCategory(Guid categoryId)
    {
        Guard.AgainstEmpty(categoryId, nameof(categoryId));

        CategoryId = categoryId;
    }

    public void ChangeUnit(Unit unit)
    {
        Unit = unit;
    }

    public void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
}
