using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.ValueObjects;

namespace FinanceTracker.TestHelpers;

public static class TestDataFactory
{
    public static readonly DateOnly DefaultDate = new DateOnly(2026, 8, 1);

    public static User CreateUser(
        string firstName,
        string lastName,
        string? email,
        UserRole role,
        string? phone
    )
    {
        var userEmail = email ?? $"{firstName}_{lastName}@gmail.com";

        var user = User.Create(firstName, lastName, userEmail, role, phone);
        user.SetPasswordHash(userEmail);

        return user;
    }

    public static Expense CreateExpense(
        Guid userId,
        Guid? sharedGroupId,
        Guid? shopId,
        decimal amount,
        Currency currency,
        DateOnly? date
    )
    {
        return Expense.Create(
            userId,
            sharedGroupId,
            shopId,
            Money.Create(amount, currency),
            date ?? DefaultDate
        );
    }

    public static Retailer CreateRetailer(string name)
    {
        return Retailer.Create(name);
    }

    public static Shop CreateShop(string name, Guid? retailerId, Address? address)
    {
        return Shop.Create(name, retailerId, address);
    }

    public static ItemCategory CreateItemCategory(string name)
    {
        return ItemCategory.Create(name);
    }

    public static Item CreateItem(string name, Guid categoryId, Unit unit = Unit.Piece)
    {
        return Item.Create(name, categoryId, unit);
    }
}
