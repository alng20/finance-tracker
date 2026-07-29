namespace FinanceTracker.Infrastructure.Options;

public sealed class UserAdminOptions
{
    public const string SectionName = "UserAdmin";

    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
