using FinanceTracker.Infrastructure.Options;
using FinanceTracker.Infrastructure.Persistence.Seed;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FinanceTracker.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();

        await context.Database.MigrateAsync();

        var options = services.GetRequiredService<IOptions<DatabaseSeederOptions>>().Value;
        if (options.IsEnabled)
        {
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

            await seeder.SeedAsync(context);
        }
    }
}
