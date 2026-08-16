namespace FinanceTracker.Infrastructure.Options;

public sealed class DatabaseSeederOptions
{
    public const string SectionName = "DatabaseSeeder";

    public bool IsEnabled { get; init; } = false;
}
