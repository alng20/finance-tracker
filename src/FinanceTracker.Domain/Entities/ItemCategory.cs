using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

// TODO: Custom user category => add UserId
public class ItemCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;

    public ItemCategory(Guid id, string name)
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(name, nameof(name));

        Id = id;
        Name = name;
    }

    public void ChangeName(string name)
    {
        Guard.AgainstEmpty(name, nameof(name));

        Name = name;
    }
}
