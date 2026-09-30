namespace FinanceTracker.Api.Common.Options;

public sealed class AppFrwdHeadersOptions
{
    public const string SectionName = "ForwardedHeaders";

    public string TrustedProxyNetwork { get; init; } = string.Empty;
}
