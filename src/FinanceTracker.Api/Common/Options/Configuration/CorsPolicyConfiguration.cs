using FinanceTracker.Api.Common.Options;

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace FinanceTracker.Api.Common.Configuration;

public sealed class CorsPolicyConfiguration(IOptions<CorsPolicyOptions> corsPolicyOptions)
    : IConfigureOptions<CorsOptions>
{
    private readonly CorsPolicyOptions _corsPolicyOptions = corsPolicyOptions.Value;

    public void Configure(CorsOptions options)
    {
        options.AddPolicy(
            "Frontend",
            policy =>
            {
                policy
                    .WithOrigins(_corsPolicyOptions.AllowedOrigins.ToArray())
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
        );
    }
}
