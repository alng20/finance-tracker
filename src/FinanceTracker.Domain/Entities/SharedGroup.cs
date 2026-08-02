using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;

namespace FinanceTracker.Domain.Entities;

public class SharedGroup
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Guid OwnerId { get; private set; }
    public User Owner { get; private set; } = null!;

    public DateTimeOffset? DeletedAt { get; private set; }

    private readonly List<SharedGroupMember> _members = new();
    public IReadOnlyCollection<SharedGroupMember> Members => _members;

    public SharedGroup(Guid id, string name, Guid ownerId)
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(name, nameof(name));
        Guard.AgainstEmpty(ownerId, nameof(ownerId));

        Id = id;
        Name = name;
        OwnerId = ownerId;

        AddOwner();
    }

    private void AddOwner()
    {
        _members.Add(
            new SharedGroupMember(
                Id,
                OwnerId,
                SharedGroupMember.GetOwnerPermission()
            )
        );
    }

    public void Rename(string name)
    {
        Guard.AgainstEmpty(name, nameof(name));

        Name = name;
    }

    public void AddMember(Guid userId, Permission permission)
    {
        if (_members.Any(x => x.UserId == userId))
        {
            throw new DomainException("User already exists in group");
        }

        SharedGroupMember member = new SharedGroupMember(
            Id,
            userId,
            permission
        );
        _members.Add(member);
    }

    public void RemoveMember(Guid userId)
    {
        Guard.AgainstEmpty(userId, nameof(userId));

        if (userId == OwnerId)
        {
            if (_members.Count == 1)
            {
                DeletedAt = DateTimeOffset.UtcNow;
                return;
            }
            throw new DomainException(
                "Transfere ownership before removing the owner"
            );
        }

        SharedGroupMember? member = _members.Find(x => x.UserId == userId);
        if (member == null)
        {
            throw new DomainException("User is not a member of this group");
        }
        _members.Remove(member);
    }

    public void TransferOwnership(Guid newOwnerId)
    {
        var newOnwer = _members.Find(x => x.UserId == newOwnerId);
        if (newOnwer == null)
        {
            throw new DomainException("New owner must be a group member");
        }
        OwnerId = newOwnerId;
        newOnwer.UpdatePermission(SharedGroupMember.GetOwnerPermission());

        SharedGroupMember? oldOwner = _members.Find(x => x.UserId == OwnerId);
        if (oldOwner == null)
        {
            throw new DomainException("Old owner doesn't exist in group");
        }
        oldOwner.UpdatePermission(Permission.Read | Permission.Write);
    }

    public void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
}
