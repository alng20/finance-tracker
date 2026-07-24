using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Item
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public ItemCategory Category { get; private set; }
    public Unit Unit { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    static public Item Create(string name, Guid categoryId, Unit unit)
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
        DeletedAt = DateTime.UtcNow;
    }
}
