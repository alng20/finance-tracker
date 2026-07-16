using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.ValueObjects;

public record Address
{
    public string Country { get; }
    public string City { get; }

    public Address(string country, string city)
    {
        Guard.AgainstEmpty(country, nameof(country));
        Guard.AgainstEmpty(city, nameof(city));

        Country = country;
        City = city;
    }
}
