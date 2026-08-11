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
    public DateOnly Date { get; private set; }

    private readonly List<ExpenseDetail> _details = new();
    public IReadOnlyCollection<ExpenseDetail> Details => _details;

    private Expense() { }

    public static Expense Create(
        Guid userId,
        Guid? sharedGroupId,
        Guid? shopId,
        Money totalAmount,
        DateOnly date
    )
    {
        return new Expense(Guid.NewGuid(), userId, sharedGroupId, shopId, totalAmount, date);
    }

    private Expense(
        Guid id,
        Guid userId,
        Guid? sharedGroupId,
        Guid? shopId,
        Money totalAmount,
        DateOnly date
    )
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(userId, nameof(userId));
        Guard.AgainstNull(totalAmount, nameof(totalAmount));
        Guard.AgainstNegative(totalAmount.Amount, nameof(totalAmount));
        Guard.AgainstFutureDate(date, nameof(date));

        Id = id;
        UserId = userId;
        SharedGroupId = sharedGroupId;
        ShopId = shopId;
        TotalAmount = totalAmount;
        Date = date;
    }

    public void AssignShop(Guid shopId)
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
        Guard.AgainstNull(amount, nameof(amount));
        Guard.AgainstNegative(amount.Amount, nameof(amount));

        TotalAmount = amount;
    }

    public ExpenseDetail AddDetail(Guid itemId, Money totalPrice, decimal quantity, decimal discountPercent)
    {
        Guard.AgainstNull(totalPrice, nameof(totalPrice));

        if (totalPrice.Currency != TotalAmount.Currency)
        {
            throw new DomainException("Expense detail currency must match expense currency");
        }

        ExpenseDetail detail = ExpenseDetail.Create(
            Id,
            itemId,
            totalPrice,
            quantity,
            discountPercent
        );

        _details.Add(detail);
        
        return detail;
    }

    public void DeleteDetail(Guid detailId)
    {
        Guard.AgainstEmpty(detailId, nameof(detailId));
        
        var detail = GetDetail(detailId);
        _details.Remove(detail);
    }
    
    public ExpenseDetail UpdateDetail(Guid detailId, Guid itemId, Money totalPrice, decimal quantity, decimal discountPercent)
    {
        if (totalPrice.Currency != TotalAmount.Currency)
        {
            throw new DomainException("Expense detail currency must match expense currency");
        }

        ExpenseDetail detail = GetDetail(detailId);
        detail.UpdateItemId(itemId);
        detail.UpdateTotalPrice(totalPrice);
        detail.UpdateQuantity(quantity);
        detail.UpdateDiscount(discountPercent);
        
        return detail;
    }

    public void RemoveDetail(Guid detailId)
    {
        ExpenseDetail detail = GetDetail(detailId);

        _details.Remove(detail);
    }

    private ExpenseDetail GetDetail(Guid detailId)
    {
        ExpenseDetail? detail = _details.FirstOrDefault(x => x.Id == detailId);
        if (detail == null)
        {
            throw new DomainException("ExpenseDetail doesn't exists");
        }

        return detail;
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

    public void UpdateSharedGroup(Guid sharedGroupId)
    {
        Guard.AgainstEmpty(sharedGroupId, nameof(sharedGroupId));

        SharedGroupId = sharedGroupId;
    }

    public void RemoveSharedGroup()
    {
        SharedGroupId = null;
    }

    public void UpdateDate(DateOnly date)
    {
        Guard.AgainstNull(date, nameof(date));
        Guard.AgainstFutureDate(date, nameof(date));

        Date = date;
    }

    public void Update(Guid? shopId, Guid? sharedGroupId, Money totalAmount, DateOnly date)
    {
        Guard.AgainstNull(totalAmount, nameof(totalAmount));
        Guard.AgainstNegative(totalAmount.Amount, nameof(totalAmount));
        Guard.AgainstFutureDate(date, nameof(date));

        ShopId = shopId;
        SharedGroupId = sharedGroupId;
        TotalAmount = totalAmount;
        Date = date;
    }
}
