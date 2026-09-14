using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Authentication;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Users.Authentication.Refresh;
using FinanceTracker.Application.Users.DTOs;

using MediatR;

namespace FinanceTracker.Application.Users.Authentication.RefreshToken;

public class RefreshTokenHandler(
    IRefreshTokenService refreshTokenService,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenService jwtTokenService,
    ICurrentRequestService currentRequestService,
    IUnitOfWork unitOfWork
) : IRequestHandler<RefreshTokenCommand, RefreshTokenResultDto>
{
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly ICurrentRequestService _currentRequestService = currentRequestService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RefreshTokenResultDto> Handle(
        RefreshTokenCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var hash = _refreshTokenService.HashToken(cmd.RefreshToken);

        var refreshToken = await _refreshTokenRepository.FindByHashWithUserAsync(
            hash,
            cancellationToken
        );
        if (refreshToken == null)
        {
            throw new UnauthorizedException("Invalid refresh token");
        }

        if (!refreshToken.IsValid())
        {
            throw new UnauthorizedException("Refresh token is invalid: expired, revoked or used");
        }

        refreshToken.MarkUsed();

        var newAccessToken = _jwtTokenService.GenerateToken(refreshToken.User);
        var newRefreshToken = _refreshTokenService.CreateRefreshToken(
            refreshToken.User,
            _currentRequestService.IpAddress,
            _currentRequestService.UserAgent
        );

        _refreshTokenRepository.Add(newRefreshToken.Entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResultDto(
            new TokenResult(newAccessToken.Token, newAccessToken.ExpiresAt),
            new TokenResult(newRefreshToken.Token, newRefreshToken.Entity.ExpiresAt),
            refreshToken.User.FirstName,
            refreshToken.User.LastName
        );
    }
}
