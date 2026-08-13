using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Infrastructure.Services;

public class CurrencyConverter() : ICurrencyConverter
{
    // TODO: Implement and use CurrencyRateProvider
    private readonly Dictionary<(Currency From, Currency To), decimal> _rates = new()
    {
        [(Currency.USD, Currency.NZD)] = 1.71m,
        [(Currency.NZD, Currency.USD)] = 0.59m,

        [(Currency.RUB, Currency.NZD)] = 0.020m,
        [(Currency.NZD, Currency.RUB)] = 48.96m,
    };

    public decimal Convert(decimal amount, Currency from, Currency to)
    {
        if (from == to)
        {
            return amount;
        }

        if (!_rates.TryGetValue((from, to), out var rate))
        {
            throw new InvalidOperationException($"Failed to convert from {from} to {to}");
        }

        return amount * rate;
    }
}
