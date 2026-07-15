using FinanceTracker.Domain.Exceptions;

namespace FinanceTracker.Domain.Common;

public static class Guard
{
    public static void AgainstEmpty(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Retailer name cannot be empty");
        }
    }
    public static void AgainstEmpty(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Retailer Id cannot be empty");
        }
    }
}
