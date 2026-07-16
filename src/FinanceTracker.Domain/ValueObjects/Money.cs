using System.Security.AccessControl;

using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.ValueObjects;

// TODO: Support different currency
public record Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException($"Amount {amount} can't be less than 0");
        }

        Amount = amount;
        Currency = currency;
    }

    public static Money operator /(Money left, Money right)
    {
        return new Money(left.Amount / right.Amount, left.Currency);
    }

    public static Money operator /(Money money, decimal value)
    {
        return new Money(money.Amount / value, money.Currency);
    }

    public int CompareTo(Money other)
    {
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
