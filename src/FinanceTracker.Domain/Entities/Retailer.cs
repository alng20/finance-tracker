using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class Retailer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;

    public ICollection<Shop> Shops { get; private set; } = new List<Shop>();

    public Retailer(Guid id, string name)
    {
        Guard.AgainstEmpty(name);
        Guard.AgainstEmpty(id);

        Id = id;
        Name = name;
    }

    public void Rename(string name)
    {
        Guard.AgainstEmpty(name);

        Name = name;
    }
}
