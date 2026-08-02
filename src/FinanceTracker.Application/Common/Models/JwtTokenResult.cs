namespace FinanceTracker.Application.Common.Models;

public record JwtTokenResult(string Token, DateTimeOffset ExpiresAt);
