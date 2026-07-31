using FinanceTracker.Application.Common.Interfaces.Authentication;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly FinanceTrackerDbContext _ctx;

    public RefreshTokenRepository(FinanceTrackerDbContext ctx)
    {
        _ctx = ctx;
    }

    public void Add(RefreshToken refreshToken)
    {
        _ctx.RefreshTokens.Add(refreshToken);
    }

    public async Task<RefreshToken?> FindByHashAsync(
        string hash,
        CancellationToken cancellationToken
    )
    {
        return await _ctx.RefreshTokens.FirstOrDefaultAsync(
            x => x.TokenHash == hash,
            cancellationToken
        );
    }

    public async Task<RefreshToken?> FindByHashWithUserAsync(
        string hash,
        CancellationToken cancellationToken
    )
    {
        return await _ctx
            .RefreshTokens.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == hash && x.User.DeletedAt == null, cancellationToken);
    }
}
