using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class Retailer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;

    public DateTime? DeletedAt { get; private set; }

    public Retailer(Guid id, string name)
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(name, nameof(name));

        Id = id;
        Name = name;
    }

    public void Rename(string name)
    {
        Guard.AgainstEmpty(name, nameof(name));

        Name = name;
    }

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
