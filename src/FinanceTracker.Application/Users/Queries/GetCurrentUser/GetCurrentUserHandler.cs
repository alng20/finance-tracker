using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Users.DTOs;

using MediatR;

namespace FinanceTracker.Application.Queries.GetCurrentUser;

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IUserRepository _userRepository;

    public GetCurrentUserHandler(ICurrentUserService currentUser, IUserRepository userRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByIdAsync(_currentUser.UserId, cancellationToken);

        return new UserDto(user.Id, user.FirstName, user.LastName, user.Email);
    }
}
