using FinanceTracker.Infrastructure.Persistence;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Testcontainers.PostgreSql;

namespace FinanceTracker.Api.Tests;

public class FinanceTrackerApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .WithDatabase("finance_tracker_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public string TestUserRole { get; set; } = "User";

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<FinanceTrackerDbContext>();
            services.RemoveAll<DbContextOptions<FinanceTrackerDbContext>>();

            services.AddDbContext<FinanceTrackerDbContext>(options =>
            {
                options.UseNpgsql(_postgres.GetConnectionString());
            });
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                    "Test",
                    _ => { }
                );
        });
    }

    // TODO: Make it common for all tests
    public async Task ClearShopsAsync()
    {
        await using var scope = Services.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();

        await context.Shops.ExecuteDeleteAsync();
    }

    public HttpClient CreateAdminClient()
    {
        var client = CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Role", "Admin");

        return client;
    }

    public HttpClient CreateUserClient()
    {
        var client = CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Role", "User");

        return client;
    }
}
