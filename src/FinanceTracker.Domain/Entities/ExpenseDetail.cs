using System.Globalization;

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

    private ExpenseDetail() { }

    static internal ExpenseDetail Create(
        Guid expenseId,
        Guid itemId,
        Money totalPrice,
        decimal quantity,
        decimal discountPercent
    )
    {
        return new ExpenseDetail(
            Guid.NewGuid(),
            expenseId,
            itemId,
            totalPrice,
            quantity,
            discountPercent
        );
    }

    private ExpenseDetail(
        Guid id,
        Guid expenseId,
        Guid itemId,
        Money totalPrice,
        decimal quantity,
        decimal discountPercent
    )
    {
        Guard.AgainstEmpty(id, nameof(id));
        Guard.AgainstEmpty(expenseId, nameof(expenseId));
        Guard.AgainstEmpty(itemId, nameof(itemId));
        Guard.GreaterThan(quantity, 0, nameof(quantity));
        Guard.AgainstNull(totalPrice, nameof(totalPrice));
        Guard.GreaterThan(totalPrice.Amount, 0, nameof(totalPrice));
        Guard.InRange(discountPercent, 0, 100, nameof(discountPercent));

        Id = id;
        ExpenseId = expenseId;
        ItemId = itemId;

        TotalPrice = totalPrice;
        Quantity = quantity;
        DiscountPercent = discountPercent;

        CalculatePrices();
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

    internal void UpdateTotalPrice(Money totalPrice)
    {
        Guard.AgainstNull(totalPrice, nameof(totalPrice));
        Guard.GreaterThan(totalPrice.Amount, 0, nameof(totalPrice));

        TotalPrice = totalPrice;
        CalculatePrices();
    }

    internal void UpdateQuantity(decimal quantity)
    {
        Guard.GreaterThan(quantity, 0, nameof(quantity));

        Quantity = quantity;
        CalculatePrices();
    }

    internal void UpdateDiscount(decimal discountPercent)
    {
        Guard.GreaterThan(discountPercent, 0, nameof(discountPercent));

        DiscountPercent = discountPercent;
        CalculatePrices();
    }
}
