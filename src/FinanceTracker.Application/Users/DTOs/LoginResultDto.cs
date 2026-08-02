namespace FinanceTracker.Application.Users.DTOs;

public record LoginResultDto(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiresAt
);
