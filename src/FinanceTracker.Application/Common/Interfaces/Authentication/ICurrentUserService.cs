using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Common.Interfaces.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }

    string? Email { get; }

    UserRole Role { get; }

    bool IsAuthenticated { get; }
}
