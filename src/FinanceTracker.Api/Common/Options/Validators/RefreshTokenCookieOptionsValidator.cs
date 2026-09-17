using Microsoft.Extensions.Options;

namespace FinanceTracker.Api.Common.Options.Validators;

public class RefreshTokenCookieOptionsValidator : IValidateOptions<RefreshTokenCookieOptions>
{
    private static readonly IReadOnlySet<string> ValidSameSites = new HashSet<string>(
        StringComparer.Ordinal
    )
    {
        "Lax",
        "Strict",
        "None",
    };
    private const string ApiPath = "/api/auth";

    public ValidateOptionsResult Validate(string? name, RefreshTokenCookieOptions options)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(options.Name))
        {
            errors.Add("RefreshTokenCookie:Name is invalid");
        }
        if (!options.HttpOnly)
        {
            errors.Add("RefreshTokenCookie:HttpOnly is invalid");
        }
        if (!ValidSameSites.Contains(options.SameSite))
        {
            errors.Add("RefreshTokenCookie:SameSite is invalid");
        }
        if (options.SameSite == "None" && !options.Secure)
        {
            errors.Add(
                "RefreshTokenCookie:SameSite and RefreshTokenCookie.Secure combination is invalid"
            );
        }
        if (string.IsNullOrWhiteSpace(options.Path) || options.Path != ApiPath)
        {
            errors.Add("RefreshTokenCookie:Path is invalid");
        }

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }
}
