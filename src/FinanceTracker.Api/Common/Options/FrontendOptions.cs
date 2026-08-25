namespace FinanceTracker.Api.Common.Options;

public sealed class CorsOptions
{
    public const string SectionName = "Cors";

    public IReadOnlyCollection<string> AllowedHosts { get; init; } = Array.Empty<string>();
}
