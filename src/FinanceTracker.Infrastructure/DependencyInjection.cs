using FinanceTracker.Application.Common.Interfaces.Authentication;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Authentication;
using FinanceTracker.Infrastructure.Options;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Persistence.Providers;
using FinanceTracker.Infrastructure.Persistence.Repositories;
using FinanceTracker.Infrastructure.Persistence.Seed;
using FinanceTracker.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Database connection string was not found.");

        services.AddDbContext<FinanceTrackerDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services
            .AddOptions<UserAdminOptions>()
            .Bind(configuration.GetSection(UserAdminOptions.SectionName));
        services
            .AddOptions<DatabaseSeederOptions>()
            .Bind(configuration.GetSection(DatabaseSeederOptions.SectionName));

        services.AddScoped<UserAdminSeeder>();
        services.AddScoped<ItemCategorySeeder>();
        services.AddScoped<DatabaseSeeder>();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        services.AddScoped<IExpenseProvider, ExpenseProvider>();
        services.AddScoped<IItemProvider, ItemProvider>();
        services.AddScoped<IShopProvider, ShopProvider>();

        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
        services.AddScoped<IShopRepository, ShopRepository>();
        services.AddScoped<IRetailerRepository, RetailerRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IExpenseReportRepository, ExpenseReportRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

        return services;
    }
}
