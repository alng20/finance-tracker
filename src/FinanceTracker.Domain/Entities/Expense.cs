using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.ValueObjects;

namespace FinanceTracker.Domain.Entities;

public class Expense
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid? SharedGroupId { get; private set; }
    public SharedGroup? SharedGroup { get; private set; }
    public Guid? ShopId { get; private set; }
    public Shop? Shop { get; private set; }
    public Money TotalAmount { get; private set; } = null!;
    public DateTime Date { get; private set; }

    private readonly List<ExpenseDetail> _details = new();
    public IReadOnlyCollection<ExpenseDetail> Details => _details;

    private Expense() { }

    public Expense(
        Guid id,
        Guid userId,
        Guid? sharedGroupId,
        Guid? shopId,
        Money totalAmount,
        DateTime date
    )
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(userId, nameof(userId));

        Id = id;
        UserId = userId;
        SharedGroupId = sharedGroupId;
        ShopId = shopId;
        TotalAmount = totalAmount;
        Date = date;
    }

    public void ChangeShop(Guid shopId)
    {
        Guard.AgainstEmpty(shopId, nameof(shopId));

        ShopId = shopId;
    }

    public void RemoveShop()
    {
        ShopId = null;
    }

    public void UpdateAmount(Money amount)
    {
        TotalAmount = amount;
    }

    public void AddDetail(ExpenseDetail detail)
    {
        if (_details.Any(x => x.Id == detail.Id))
        {
            throw new DomainException("ExpenseDetail already exists in group");
        }

        detail.AssignExpense(Id);
        _details.Add(detail);
    }

    public void RemoveDetail(ExpenseDetail detail)
    {
        _details.Remove(detail);
    }

    public void AssignSharedGroup(Guid sharedGroupId)
    {
        Guard.AgainstEmpty(sharedGroupId, nameof(sharedGroupId));
        if (SharedGroupId.HasValue)
        {
            throw new DomainException("Expense already has group");
        }

        SharedGroupId = sharedGroupId;
    }

    public void RemoveSharedGroup()
    {
        SharedGroupId = null;
    }
}
