using Microsoft.Extensions.Options;

namespace FinanceTracker.Api.Common.Options.Validators;

public class CorsPolicyOptionsValidator : IValidateOptions<CorsPolicyOptions>
{
    public ValidateOptionsResult Validate(string? name, CorsPolicyOptions options)
    {
        List<string> errors = new();
        if (options.AllowedOrigins.Count == 0)
        {
            errors.Add("CorsPolicyOptions:AllowedOrigins is empty");
        }
        foreach (var host in options.AllowedOrigins)
        {
            if (
                string.IsNullOrWhiteSpace(host)
                || !Uri.TryCreate(host, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            )
            {
                errors.Add($"CorsPolicyOptions:AllowedOrigins.host {host} is invalid");
            }
        }

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }
}
