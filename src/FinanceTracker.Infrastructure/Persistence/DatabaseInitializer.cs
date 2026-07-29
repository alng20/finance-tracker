using FinanceTracker.Infrastructure.Persistence.Seed;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(
        IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<FinanceTrackerDbContext>();

        await context.Database.MigrateAsync();

        var seeder = scope.ServiceProvider
            .GetRequiredService<DatabaseSeeder>();

        await seeder.SeedAsync(context);
    }
}