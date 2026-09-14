using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Users.DTOs;
using FinanceTracker.Domain.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Application.Users.Authentication.Register;

public class RegisterHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher<User> passwordHasher
) : IRequestHandler<RegisterCommand, UserDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<UserDto> Handle(RegisterCommand cmd, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.FindByEmailAsync(cmd.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new ConflictException($"User with email {cmd.Email} already exists");
        }

        var user = User.Create(
            cmd.FirstName,
            cmd.LastName,
            cmd.Email,
            Domain.Enums.UserRole.User,
            cmd.Phone
        );
        var passwordHash = _passwordHasher.HashPassword(user, cmd.Password);
        user.SetPasswordHash(passwordHash);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserDto(user.Id, user.FirstName, user.LastName, user.Email, user.Role);
    }
}
