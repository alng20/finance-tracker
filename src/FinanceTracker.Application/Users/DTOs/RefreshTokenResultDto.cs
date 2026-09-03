using FinanceTracker.Application.Common.Models;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Users.DTOs;

public record RefreshTokenResultDto(TokenResult AccessToken, TokenResult RefreshToken, string FirstName, string LastName);
