using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Common.Interfaces.Services;

public interface ICurrencyConverter
{
    decimal Convert(decimal amount, Currency src, Currency dst);
}
