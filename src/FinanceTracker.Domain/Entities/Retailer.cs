using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class Retailer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;

    public DateTimeOffset? DeletedAt { get; private set; }

    public static Retailer Create(string name)
    {
        return new Retailer(Guid.NewGuid(), name);
    }

    private Retailer() { }

    private Retailer(Guid id, string name)
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

    public void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
}
