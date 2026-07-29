namespace FinanceTracker.Application.Users.DTOs;

public record LoginResultDto(string Token, DateTime ExpiresAt);
