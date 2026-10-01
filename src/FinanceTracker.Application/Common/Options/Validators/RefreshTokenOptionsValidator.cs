using Microsoft.Extensions.Options;

namespace FinanceTracker.Application.Common.Options.Validators;

public class RefreshTokenOptionsValidator : IValidateOptions<RefreshTokenOptions>
{
    private const int MinTokenLength = 64;

    private const int MinExpirationDays = 1;
    private const int MaxExpirationDays = 30;

    public ValidateOptionsResult Validate(string? name, RefreshTokenOptions options)
    {
        List<string> errors = new();
        if (options.ExpirationDays < MinExpirationDays || options.ExpirationDays > MaxExpirationDays)
        {
            errors.Add("RefreshToken:ExpirationDays is invalid");
        }
        if (options.TokenLength < MinTokenLength)
        {
            errors.Add("RefreshToken:TokenLength is invalid");
        }

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }
}
