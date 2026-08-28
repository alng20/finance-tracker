using MediatR;

namespace FinanceTracker.Application.Users.Authentication.Logout;

public record LogoutCommand(string RefreshToken) : IRequest;
