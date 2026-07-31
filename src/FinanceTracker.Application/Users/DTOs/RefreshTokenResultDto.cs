namespace FinanceTracker.Application.Users.DTOs;

public record RefreshTokenResultDto(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAt);
