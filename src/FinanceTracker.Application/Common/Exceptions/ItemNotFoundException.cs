namespace FinanceTracker.Application.Common.Exceptions;

public class ItemNotFoundException : Exception
{
    public Guid ItemId { get; }

    public ItemNotFoundException(Guid itemId)
        : base($"Item {itemId} is not found")
    {
        ItemId = itemId;
    }
}
