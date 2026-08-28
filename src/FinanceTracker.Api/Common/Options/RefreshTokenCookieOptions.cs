namespace FinanceTracker.Api.Common.Options;

public sealed class RefreshTokenCookieOptions
{
    public const string SectionName = "RefreshTokenCookies";

    public string Name { get; set; } = "refreshToken";
    public bool HttpOnly { get; set; } = true;
    public bool Secure { get; set; }
    public string SameSite { get; set; } = "Lax";
    public string Path { get; set; } = "/api/auth";
}
