using FinanceTracker.Application.Users.DTOs;

using MediatR;

namespace FinanceTracker.Application.Users.Authentication.Refresh;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResultDto>;
