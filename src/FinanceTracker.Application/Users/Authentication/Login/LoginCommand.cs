using FinanceTracker.Application.Users.DTOs;

using MediatR;

namespace FinanceTracker.Application.Users.Authentication.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResultDto>;
