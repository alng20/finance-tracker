using FinanceTracker.Application.Users.DTOs;
using MediatR;

namespace FinanceTracker.Application.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<UserDto>;
