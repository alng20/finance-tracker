using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Options;

namespace FinanceTracker.Api.Common.Options.Validators;

public class AppFrwdHeadersOptionsValidator : IValidateOptions<AppFrwdHeadersOptions>
{
    public ValidateOptionsResult Validate(string? name, AppFrwdHeadersOptions options)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(options.TrustedProxyNetwork))
        {
            errors.Add("ForwardedHeaders:TrustedProxyNetwork is empty");
        }
        else
        {
            var parts = options.TrustedProxyNetwork.Split('/');
            if (parts.Length != 2)
            {
                errors.Add("TrustedProxyNetwork must have format <IP/prefix>");
            }
            else
            {
                if (!IPAddress.TryParse(parts[0], out var ipAddress))
                {
                    errors.Add("TrustedProxyNetwork contains an invalid IP address.");
                }

                if (ipAddress?.AddressFamily != AddressFamily.InterNetwork)
                {
                    errors.Add("TrustedProxyNetwork must contain an IPv4 address.");
                }

                if (!int.TryParse(parts[1], out var prefixLength))
                {
                    errors.Add("TrustedProxyNetwork contains an invalid prefix length.");
                }
                else if (prefixLength is < 0 or > 32)
                {
                    errors.Add("IPv4 prefix length must be between 0 and 32.");
                }
                else if (ipAddress != null)
                {
                    validateNetwork(options.TrustedProxyNetwork, ipAddress, prefixLength, errors);
                }
            }
        }

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }

    private void validateNetwork(
        string network,
        IPAddress ipAddress,
        int prefixLength,
        List<string> errors
    )
    {
        var bytes = ipAddress.GetAddressBytes();

        var mask = prefixLength == 0 ? 0u : uint.MaxValue << (32 - prefixLength);

        var ip = ((uint)bytes[0] << 24) | ((uint)bytes[1] << 16) | ((uint)bytes[2] << 8) | bytes[3];

        var networkAddress = ip & mask;

        var networkBytes = new byte[]
        {
            (byte)(networkAddress >> 24),
            (byte)(networkAddress >> 16),
            (byte)(networkAddress >> 8),
            (byte)networkAddress,
        };

        var expectedNetworkAddress = new IPAddress(networkBytes);

        if (!ipAddress.Equals(expectedNetworkAddress))
        {
            errors.Add(
                $"'{network}' is not a valid network address. "
                    + $"Expected '{expectedNetworkAddress}/{prefixLength}'."
            );
        }
    }
}
