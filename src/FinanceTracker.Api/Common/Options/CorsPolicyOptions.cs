namespace FinanceTracker.Api.Common.Options;

public sealed class CorsPolicyOptions
{
    public const string SectionName = "Cors";

    public IReadOnlyCollection<string> AllowedOrigins { get; init; } = Array.Empty<string>();
}
