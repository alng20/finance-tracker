using FinanceTracker.Application.Users.DTOs;

using MediatR;

namespace FinanceTracker.Application.Users.Authentication.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string? Phone
) : IRequest<UserDto>;
