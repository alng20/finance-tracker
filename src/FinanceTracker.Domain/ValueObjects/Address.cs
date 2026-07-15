using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.ValueObjects;

public record Address
{
    public string Country;
    public string City;

    public Address(string country, string city)
    {
        Guard.AgainstEmpty(country);
        Guard.AgainstEmpty(city);

        Country = country;
        City = city;
    }
}
