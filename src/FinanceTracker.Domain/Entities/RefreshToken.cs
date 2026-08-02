using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Exceptions;

namespace FinanceTracker.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string TokenHash { get; private set; } = null!;
    public string CreatedByIp { get; private set; } = null!;
    public string UserAgent { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public DateTimeOffset? UsedAt { get; private set; }

    public static RefreshToken Create(
        Guid userId,
        string tokenHash,
        DateTimeOffset expiresAt,
        string createdByIp,
        string userAgent
    )
    {
        return new RefreshToken(
            Guid.NewGuid(),
            userId,
            tokenHash,
            expiresAt,
            createdByIp,
            userAgent
        );
    }

    private RefreshToken() { }

    private RefreshToken(
        Guid id,
        Guid userId,
        string tokenHash,
        DateTimeOffset expiresAt,
        string createdByIp,
        string userAgent
    )
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(userId, nameof(userId));
        Guard.AgainstEmpty(tokenHash, nameof(tokenHash));
        Guard.AgainstEmpty(createdByIp, nameof(createdByIp));
        Guard.AgainstEmpty(userAgent, nameof(userAgent));

        var now = DateTimeOffset.UtcNow;
        Guard.GreaterThan(expiresAt, now, nameof(expiresAt));

        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedByIp = createdByIp;
        UserAgent = userAgent;
        CreatedAt = now;
    }

    public bool IsExpired()
    {
        return DateTimeOffset.UtcNow >= ExpiresAt;
    }

    public bool IsRevoked()
    {
        return RevokedAt.HasValue;
    }

    public bool IsValid()
    {
        return !IsExpired() && !IsRevoked() && !UsedAt.HasValue;
    }

    public void Revoke()
    {
        if (RevokedAt.HasValue)
        {
            return;
        }

        RevokedAt = DateTimeOffset.UtcNow;
    }

    public void MarkUsed()
    {
        if (UsedAt.HasValue)
        {
            throw new DomainException("Refresh token was already used");
        }
        UsedAt = DateTimeOffset.UtcNow;
    }
}
