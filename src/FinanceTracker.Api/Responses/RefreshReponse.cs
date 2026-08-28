namespace FinanceTracker.Api.Responses;

public record RefreshResponse(string AccessToken, DateTimeOffset AccessTokenExpiresAt, string FirstName, string LastName);
