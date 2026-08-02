namespace FinanceTracker.Application.Users.DTOs;

public record RefreshTokenResultDto(string AccessToken, string RefreshToken, DateTimeOffset AccessTokenExpiresAt);
