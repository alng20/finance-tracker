namespace FinanceTracker.Application.Common.Exceptions;

public class ItemCategoryAlreadyExistsException : Exception
{
    public Guid ItemCategoryId { get; }
    public string Name { get; }

    public ItemCategoryAlreadyExistsException(Guid itemCategoryId, string name)
        : base($"Item category {itemCategoryId} already has name {name}")
    {
        ItemCategoryId = itemCategoryId;
        Name = name;
    }
}
