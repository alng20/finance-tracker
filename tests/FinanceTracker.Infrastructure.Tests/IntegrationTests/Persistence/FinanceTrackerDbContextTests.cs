using FinanceTracker.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace FinanceTracker.Infrastructure.Tests.IntegrationTests.Persistence;

public class FinanceTrackerDbContextTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .WithDatabase("finance_tracker_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    private FinanceTrackerDbContext _context = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _context = new FinanceTrackerDbContext(options);

        await _context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task CanConnectToDatabase()
    {
        var canConnect = await _context.Database.CanConnectAsync();

        canConnect.Should().BeTrue();
    }
}
