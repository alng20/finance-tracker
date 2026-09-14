using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Seed;

public class ItemCategorySeeder
{
    private static readonly string[] DefaultCategories =
    [
        "Food",
        "Transport",
        "Health",
        "Clothes",
        "Home",
        "Entertainment",
        "Other",
    ];

    public async Task SeedAsync(FinanceTrackerDbContext context)
    {
        foreach (string categoryName in DefaultCategories)
        {
            bool exists = await context.ItemCategories.AnyAsync(x => x.Name == categoryName);

            if (!exists)
            {
                context.ItemCategories.Add(ItemCategory.Create(categoryName));
            }
        }

        await context.SaveChangesAsync();
    }
}
