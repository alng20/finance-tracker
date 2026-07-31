namespace FinanceTracker.Application.Common.Options;

public class RefreshTokenOptions
{
    public const string SectionName = "RefreshToken";
    public int ExpirationDays { get; set; } = 30;
    public int TokenLength { get; set; } = 64;
}
