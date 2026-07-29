using FinanceTracker.Application.Common.Interfaces.Authentication;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Authentication;
using FinanceTracker.Infrastructure.Options;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Persistence.Repositories;
using FinanceTracker.Infrastructure.Persistence.Seed;

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

        services.Configure<UserAdminOptions>(
            configuration.GetSection(UserAdminOptions.SectionName)
        );

        services.AddScoped<UserAdminSeeder>();
        services.AddScoped<ItemCategorySeeder>();
        services.AddScoped<DatabaseSeeder>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
