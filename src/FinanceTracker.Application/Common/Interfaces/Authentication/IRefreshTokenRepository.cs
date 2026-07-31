using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Authentication;

public interface IRefreshTokenRepository
{
    void Add(RefreshToken refreshToken);

    Task<RefreshToken?> FindByHashAsync(string hash, CancellationToken cancellationToken);
    Task<RefreshToken?> FindByHashWithUserAsync(string hash, CancellationToken cancellationToken);
}
