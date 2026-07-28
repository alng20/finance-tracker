using FluentValidation.Results;

namespace FinanceTracker.Application.Common.Exceptions;

public class AppValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public AppValidationException(IEnumerable<ValidationFailure> failures)
    {
        Errors = failures
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(f => f.ErrorMessage).ToArray());
    }
}
