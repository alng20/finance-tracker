using FinanceTracker.Application.Common.Models;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Common.Interfaces.Authentication;

public interface IJwtTokenService
{
    JwtTokenResult GenerateToken(User user);
}
