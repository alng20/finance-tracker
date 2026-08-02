using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;

namespace FinanceTracker.Domain.ValueObjects;

// TODO: Support different currency
public record Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    private Money() { }

    private static void CheckCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new DomainException(
                "Cannot operate with different currencies");
        }
    }

    static public Money Create(decimal amount, Currency currency)
    {
        return new Money(amount, currency);
    }

    private Money(decimal amount, Currency currency)
    {
        Guard.AgainstNegative(amount, nameof(amount));

        Amount = amount;
        Currency = currency;
    }

    public static Money operator /(Money money, decimal value)
    {
        return new Money(money.Amount / value, money.Currency);
    }

    public int CompareTo(Money other)
    {
        CheckCurrency(this, other);
        if (other == null)
        {
            return 1;
        }

        return Amount.CompareTo(other.Amount);
    }

    public int CompareTo(decimal amount)
    {
        return Amount.CompareTo(amount);
    }
}
