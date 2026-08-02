using System.Security.Claims;
using System.Text;
using FinanceTracker.Application.Common.Interfaces.Authentication;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Common.Options;
using FinanceTracker.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MicrosoftJwt = Microsoft.IdentityModel.JsonWebTokens;
using SystemJwt = System.IdentityModel.Tokens.Jwt;

namespace FinanceTracker.Infrastructure.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public JwtTokenResult GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(MicrosoftJwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(MicrosoftJwt.JwtRegisteredClaimNames.Email, user.Email),
            new Claim(MicrosoftJwt.JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(MicrosoftJwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(
                MicrosoftJwt.JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64
            ),
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256
        );

        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_options.ExpirationMinutes);
        var accessToken = new SystemJwt.JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: signingCredentials
        );

        var handler = new SystemJwt.JwtSecurityTokenHandler();

        return new JwtTokenResult(handler.WriteToken(accessToken), expiresAt);
    }
}
