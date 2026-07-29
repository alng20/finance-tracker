using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public string? Phone { get; private set; }

    // TODO: Add Status{Active, Blocked, Archived} instead of DeletedAt
    public DateTime? DeletedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    private readonly List<SharedGroupMember> _groupMemberships = new();
    public IReadOnlyCollection<SharedGroupMember> GroupMemberships => _groupMemberships;

    public static User Create(
        string firstName,
        string lastName,
        string email,
        UserRole role,
        string? phone
    )
    {
        return new User(Guid.NewGuid(), firstName, lastName, email, role, phone);
    }

    private User() { }

    private User(
        Guid id,
        string firstName,
        string lastName,
        string email,
        UserRole role,
        string? phone
    )
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(firstName, nameof(firstName));
        Guard.AgainstEmpty(lastName, nameof(lastName));
        Guard.AgainstEmpty(email, nameof(email));
        Guard.AgainstNull(role, nameof(role));

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
        Phone = phone;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetPasswordHash(string passwordHash)
    {
        Guard.AgainstEmpty(passwordHash, nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    public void Rename(string firstName, string lastName)
    {
        Guard.AgainstEmpty(firstName, nameof(firstName));
        Guard.AgainstEmpty(lastName, nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeEmail(string email)
    {
        Guard.AgainstEmpty(email, nameof(email));

        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePasswordHash(string passwordHash)
    {
        Guard.AgainstEmpty(passwordHash, nameof(passwordHash));

        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeRole(UserRole role)
    {
        Guard.AgainstNull(role, nameof(role));

        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignPhone(string phone)
    {
        Guard.AgainstEmpty(phone, nameof(phone));

        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemovePhone()
    {
        Phone = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete() // TODO: make class SoftDeletableEntity
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
