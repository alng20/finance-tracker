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
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public DateTime? UsedAt { get; private set; }

    public static RefreshToken Create(
        Guid userId,
        string tokenHash,
        DateTime expiresAt,
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
        DateTime expiresAt,
        string createdByIp,
        string userAgent
    )
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(userId, nameof(userId));
        Guard.AgainstEmpty(tokenHash, nameof(tokenHash));
        Guard.AgainstEmpty(createdByIp, nameof(createdByIp));
        Guard.AgainstEmpty(userAgent, nameof(userAgent));

        var now = DateTime.UtcNow;
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
        return DateTime.UtcNow >= ExpiresAt;
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

        RevokedAt = DateTime.UtcNow;
    }

    public void MarkUsed()
    {
        if (UsedAt.HasValue)
        {
            throw new DomainException("Refresh token was already used");
        }
        UsedAt = DateTime.UtcNow;
    }
}
