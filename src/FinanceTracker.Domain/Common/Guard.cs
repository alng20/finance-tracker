using FinanceTracker.Domain.Exceptions;

namespace FinanceTracker.Domain.Common;

public static class Guard
{
    public static void AgainstNull(Object value, string name)
    {
        if (value == null)
        {
            throw new DomainException($"{name} cannot be null");
        }
    }

    public static void AgainstEmpty(string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{name} cannot be empty");
        }
    }

    public static void AgainstEmpty(Guid value, string name)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException($"{name} cannot be empty");
        }
    }

    public static void AgainstNegative(decimal value, string name)
    {
        if (value < 0)
        {
            throw new DomainException($"{name} cannot be negative ");
        }
    }

    public static void InRange<Type>(Type value, Type min, Type max, string name) where Type : IComparable<Type>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
        {
            throw new DomainException($"{name} not in range [{min}, {max}] ");
        }
    }

    public static void GreaterThanOrEqualTo<Type>(Type value, Type check, string name) where Type : IComparable<Type>
    {
        if (value.CompareTo(check) < 0)
        {
            throw new DomainException($"{name} have to be greater than {check}");
        }
    }

    public static void GreaterThan<Type>(Type value, Type check, string name) where Type : IComparable<Type>
    {
        if (value.CompareTo(check) <= 0)
        {
            throw new DomainException($"{name} have to be greater than {check}");
        }
    }

    public static void AgainstFutureDate(DateOnly value, string name)
    {
        if (value > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new DomainException($"{name} is future date");
        }
    }

}
