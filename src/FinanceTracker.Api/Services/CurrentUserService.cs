using System.Security.Claims;

using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            string? value = _httpContextAccessor.HttpContext?.User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!Guid.TryParse(value, out Guid userId))
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            return userId;
        }
    }

    public string? Email => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

    public UserRole Role
    {
        get
        {
            string? value = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

            if (!Enum.TryParse<UserRole>(value, out var role))
            {
                throw new UnauthorizedException("User role is invalid.");
            }

            return role;
        }
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
