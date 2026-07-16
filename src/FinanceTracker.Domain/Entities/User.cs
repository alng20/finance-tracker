using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }

    public User(Guid id, string firstName, string lastName, string? email, string? phone)
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(firstName, nameof(firstName));
        Guard.AgainstEmpty(lastName, nameof(lastName));

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }

    public void Rename(string firstName, string lastName)
    {
        Guard.AgainstEmpty(firstName, nameof(firstName));
        Guard.AgainstEmpty(lastName, nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }

    public void AssignEmail(string email)
    {
        Guard.AgainstEmpty(email, nameof(email));

        Email = email;
    }

    public void RemoveEmail()
    {
        Email = null;
    }

    public void AssignPhone(string phone)
    {
        Guard.AgainstEmpty(phone, nameof(phone));

        Phone = phone;
    }

    public void RemovePhone()
    {
        Phone = null;
    }
}
