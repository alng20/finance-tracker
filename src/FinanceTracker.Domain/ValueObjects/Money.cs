using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;

namespace FinanceTracker.Domain.ValueObjects;

// TODO: Support different currency
public record Money
{
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }

    private Money() { }

    private static void CheckCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new DomainException(
                "Cannot operate with different currencies");
        }
    }

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
        CheckCurrency(left, right);
        return new Money(left.Amount / right.Amount, left.Currency);
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
