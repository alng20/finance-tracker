using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Authentication;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Users.DTOs;
using FinanceTracker.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Application.Users.Authentication.Login;

public class LoginHandler(
    IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher,
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService,
    IRefreshTokenRepository refreshTokenRepository,
    ICurrentRequestService currentRequestService,
    IUnitOfWork unitOfWork
) : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly ICurrentRequestService _currentRequestService = currentRequestService;
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
        }

        var accessToken = _jwtTokenService.GenerateToken(user);

        var refreshToken = _refreshTokenService.CreateRefreshToken(
            user,
            _currentRequestService.IpAddress,
            _currentRequestService.UserAgent
        );

        user.AddRefreshToken(refreshToken.Entity);
        _refreshTokenRepository.Add(refreshToken.Entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResultDto(accessToken.Token, refreshToken.Token, accessToken.ExpiresAt);
    }
}
