using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Authentication;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using MediatR;

namespace FinanceTracker.Application.Users.Authentication.Logout;

public class LogoutHandler(
    ICurrentUserService currentUser,
    IRefreshTokenService refreshTokenService,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<LogoutCommand>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(LogoutCommand cmd, CancellationToken cancellationToken)
    {
        var hash = _refreshTokenService.HashToken(cmd.RefreshToken);
        var refreshToken = await _refreshTokenRepository.FindByHashAsync(hash, cancellationToken);
        if (refreshToken is null)
        {
            return;
        }

        if (refreshToken.UserId != _currentUser.UserId)
        {
            throw new UnauthorizedException("Logout is failed, no access");
        }

        if (!refreshToken.IsRevoked())
        {
            refreshToken.Revoke();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
