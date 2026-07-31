using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Authentication;

public interface IRefreshTokenService
{
    RefreshTokenCreationResultDto CreateRefreshToken(User user, string ipAddress, string userAgent);
    string HashToken(string token);
}
