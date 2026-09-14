using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Seed;

public class DatabaseSeeder
{
    private readonly UserAdminSeeder _userAdminSeeder;
    private readonly ItemCategorySeeder _categorySeeder;

    public DatabaseSeeder(UserAdminSeeder userAdminSeeder, ItemCategorySeeder categorySeeder)
    {
        _userAdminSeeder = userAdminSeeder;
        _categorySeeder = categorySeeder;
    }

    public async Task SeedAsync(FinanceTrackerDbContext context)
    {
        await _userAdminSeeder.SeedAsync(context);
        await _categorySeeder.SeedAsync(context);
    }
}
