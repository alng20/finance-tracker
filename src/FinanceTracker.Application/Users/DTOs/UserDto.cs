using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Users.DTOs;

public record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    UserRole Role
);
