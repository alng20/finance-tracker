using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Authentication;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Users.DTOs;
using FinanceTracker.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Application.Users.Authentication.Login;

public class LoginHandler(
    IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher,
    IJwtTokenService jwtTokenService,
    IUnitOfWork unitOfWork
) : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<LoginResultDto> Handle(LoginCommand cmd, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmailAsync(cmd.Email, cancellationToken);
        if (user == null)
        {
            throw new UnauthorizedException($"Invalid credentials");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            cmd.Password
        );
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var newHash = _passwordHasher.HashPassword(user, cmd.Password);
            user.SetPasswordHash(newHash);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var token = _jwtTokenService.GenerateToken(user);

        return new LoginResultDto(token.Token, token.ExpiresAt);
    }
}
