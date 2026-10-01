using Microsoft.Extensions.Options;

namespace FinanceTracker.Application.Common.Options.Validators;

public class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    private readonly static int MinSecretKeyLength = 32;

    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            errors.Add("Jwt:Issuer is invalid");
        }
        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            errors.Add("Jwt:Audience is invalid");
        }
        if (string.IsNullOrWhiteSpace(options.SecretKey) || options.SecretKey.Length < MinSecretKeyLength)
        {
            errors.Add("Jwt:SecretKey is invalid");
        }
        if (options.ExpirationMinutes <= 0)
        {
            errors.Add("Jwt:ExpirationMinutes is invalid");
        }

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }
}
