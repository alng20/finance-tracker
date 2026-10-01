using FinanceTracker.Api.Common.Options;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;

using SysNet = System.Net;

namespace FinanceTracker.Api.Common.Configuration;

public sealed class ForwardedHeadersConfiguration(
    IOptions<AppFrwdHeadersOptions> appFrwdHeadersOptions
) : IConfigureOptions<ForwardedHeadersOptions>
{
    private readonly AppFrwdHeadersOptions _appFrwdHeadersOptions = appFrwdHeadersOptions.Value;

    public void Configure(ForwardedHeadersOptions options)
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

        var parts = _appFrwdHeadersOptions.TrustedProxyNetwork.Split('/');

        options.KnownIPNetworks.Add(
            new SysNet.IPNetwork(SysNet.IPAddress.Parse(parts[0]), int.Parse(parts[1]))
        );
    }
}
