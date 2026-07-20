using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class SharedGroupMember
{
    public Guid SharedGroupId { get; private set; }
    public SharedGroup SharedGroup { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Permission Permission { get; private set; }

    public DateTime JoinedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public bool CanRead => Permission.HasFlag(Permission.Read);
    public bool CanWrite => Permission.HasFlag(Permission.Write);
    public bool CanEdit => Permission.HasFlag(Permission.Edit);
    public bool CanDelete => Permission.HasFlag(Permission.Delete);
    public bool CanManageMembers =>
        Permission.HasFlag(Permission.ManageMembers);

    public void UpdatePermission(Permission permission)
    {
        Permission = permission;
    }

    public SharedGroupMember(
        Guid sharedGroupId,
        Guid userId,
        Permission permission
    )
    {
        Guard.AgainstEmpty(sharedGroupId, nameof(sharedGroupId));
        Guard.AgainstEmpty(userId, nameof(userId));

        SharedGroupId = sharedGroupId;
        UserId = userId;
        Permission = permission;
        JoinedAt = DateTime.UtcNow;
    }

    public static Permission GetOwnerPermission()
    {
        return Permission.Read
            | Permission.Write
            | Permission.Edit
            | Permission.Delete
            | Permission.ManageMembers;
    }

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
