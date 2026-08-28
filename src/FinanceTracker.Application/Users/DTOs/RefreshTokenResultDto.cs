using FinanceTracker.Application.Common.Models;

namespace FinanceTracker.Application.Users.DTOs;

public record RefreshTokenResultDto(TokenResult AccessToken, TokenResult RefreshToken, string FirstName, string LastName);
