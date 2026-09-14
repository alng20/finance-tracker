using FinanceTracker.Domain.Enums;
using FinanceTracker.Infrastructure.Services;

using FluentAssertions;

namespace FinanceTracker.Infrastructure.Tests.UnitTests.Services;

public class CurrencyConverterTests
{
    private readonly CurrencyConverter _converter = new();

    [Fact]
    public void Convert_SameCurrency()
    {
        decimal amount = 100m;
        var result = _converter.Convert(amount, Currency.NZD, Currency.NZD);

        result.Should().Be(amount);
    }

    [Theory]
    [InlineData(Currency.NZD, Currency.RUB, 100, 4896)]
    [InlineData(Currency.RUB, Currency.NZD, 1000, 20)]
    public void Convert_SupportedRates(Currency from, Currency to, decimal amount, decimal expected)
    {
        var result = _converter.Convert(amount, from, to);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(Currency.USD, Currency.RUB)]
    [InlineData(Currency.RUB, Currency.USD)]
    public void Convert_UnsupportedRates(Currency from, Currency to)
    {
        decimal amount = 100m;
        Action act = () => _converter.Convert(amount, from, to);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Failed to convert from {from} to {to}");
    }
}
