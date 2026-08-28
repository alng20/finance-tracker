namespace FinanceTracker.Api.Responses;

public record LoginResponse(string AccessToken, DateTimeOffset AccessTokenExpiresAt, string FirstName, string LastName);
