namespace FinanceTracker.Domain.Enums;

[Flags]
public enum Permission
{
    None = 0b00000,
    Read = 0b00001,
    Write = 0b00010,
    Edit = 0b00100,
    Delete = 0b01000,
    ManageMembers = 0b10000,
}
