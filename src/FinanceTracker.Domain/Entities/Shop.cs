using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.ValueObjects;

namespace FinanceTracker.Domain.Entities;

public class Shop
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Guid? RetailerId { get; private set; }
    public Retailer? Retailer { get; private set; }
    public Address? Address { get; private set; }

    public Shop(Guid id, string name, Guid? retailerId, Address? address)
    {
        Guard.AgainstEmpty(name);
        Guard.AgainstEmpty(id);

        Id = id;
        Name = name;
        RetailerId = retailerId;
        Address = address;
    }

    public void Rename(string name)
    {
        Guard.AgainstEmpty(name);

        Name = name;
    }

    public void AssignRetailer(Guid retailerId)
    {
        Guard.AgainstEmpty(retailerId);

        RetailerId = retailerId;
    }

    public void RemoveRetailer()
    {
        RetailerId = null;
    }
}
