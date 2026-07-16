using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.ValueObjects;

namespace FinanceTracker.Domain.Entities;

public class ExpenseDetail
{
    public Guid Id { get; private set; }
    public Guid ExpenseId { get; private set; }
    public Expense Expense { get; private set; } = null!;
    public Guid ItemId { get; private set; }
    public Item Item { get; private set; } = null!;
    public Money TotalPrice { get; private set; } = null!;
    public Money UnitPrice { get; private set; } = null!;
    public Money? UnitDiscountPrice { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal DiscountPercent { get; private set; }

    public ExpenseDetail(Guid id, Guid itemId, Money total, decimal quantity, decimal discountPercent)
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(itemId, nameof(itemId));
        Guard.GreaterThan(quantity, 0, nameof(quantity));
        Guard.GreaterThan(total.Amount, 0, nameof(total));
        Guard.InRange(discountPercent, 0, 100, nameof(discountPercent));

        Id = id;
        ItemId = itemId;

        TotalPrice = total;
        Quantity = quantity;
        DiscountPercent = discountPercent;

        CalculatePrices();
    }

    public void AssignExpense(Guid expenseId)
    {
        if (ExpenseId != Guid.Empty)
        {
            throw new DomainException("ExpenseId is already set");
        }

        ExpenseId = expenseId;
    }

    private void CalculatePrices()
    {
        if (DiscountPercent == 0)
        {
            UnitPrice = TotalPrice / Quantity;
            UnitDiscountPrice = null;
        }
        else
        {
            Money OriginalTotalPrice = TotalPrice / (1 - DiscountPercent / 100);
            UnitPrice = OriginalTotalPrice / Quantity;
            UnitDiscountPrice = TotalPrice / Quantity;
        }
    }

    public void UpdateTotalPrice(Money total)
    {
        Guard.GreaterThan(total.Amount, 0, nameof(total));

        TotalPrice = total;
        CalculatePrices();
    }

    public void UpdateQuantity(decimal quantity)
    {
        Guard.GreaterThan(quantity, 0, nameof(quantity));

        Quantity = quantity;
        CalculatePrices();
    }

    public void UpdateDiscount(decimal discountPercent)
    {
        Guard.GreaterThan(discountPercent, 0, nameof(discountPercent));

        DiscountPercent = discountPercent;
        CalculatePrices();
    }
}
