using System.Security.Cryptography;
using System.Text;

using FinanceTracker.Application.Common.Interfaces.Authentication;
using FinanceTracker.Application.Common.Options;
using FinanceTracker.Domain.Entities;

using Microsoft.Extensions.Options;

namespace FinanceTracker.Infrastructure.Authentication;

public class RefreshTokenService(IOptions<RefreshTokenOptions> options) : IRefreshTokenService
{
    private readonly RefreshTokenOptions _options = options.Value;

    public RefreshTokenCreationResultDto CreateRefreshToken(User user, string ipAddress, string userAgent)
    {
        var token = GenerateToken();
        var hash = HashToken(token);

        var refreshToken = RefreshToken.Create(
            user.Id,
            hash,
            DateTimeOffset.UtcNow.AddDays(_options.ExpirationDays),
            ipAddress,
            userAgent
        );
        return new RefreshTokenCreationResultDto(refreshToken, token);
    }

    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }

    private string GenerateToken()
    {
        var bytes = new byte[_options.TokenLength];

        using var random = RandomNumberGenerator.Create();

        random.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }
}
