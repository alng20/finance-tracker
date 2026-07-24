namespace FinanceTracker.Application.Common.Exceptions;

public class ItemCategoryNotFoundException : Exception
{
    public Guid ItemCategoryId { get; }

    public ItemCategoryNotFoundException(Guid itemCategoryId)
        : base($"Item category {itemCategoryId} is not found")
    {
        ItemCategoryId = itemCategoryId;
    }
}
