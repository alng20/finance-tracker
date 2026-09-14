using FinanceTracker.Domain.Enums;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Persistence.Repositories;
using FinanceTracker.TestHelpers;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Testcontainers.PostgreSql;

namespace FinanceTracker.Infrastructure.Tests.IntegrationTests.Persistence;

public class ExpenseReportRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .WithDatabase("finance_tracker_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private FinanceTrackerDbContext _context = null!;
    private ExpenseReportRepository _repository = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _context = new FinanceTrackerDbContext(options);

        await _context.Database.MigrateAsync();

        _repository = new ExpenseReportRepository(_context);
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task GetExpensesAmountByDateAsync_ReturnsExpensesForUser()
    {
        var user = TestDataFactory.CreateUser("a", "b", null, UserRole.User, null);
        var expense = TestDataFactory.CreateExpense(user.Id, null, null, 100, Currency.NZD, null);

        _context.Users.Add(user);
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        var result = await _repository.GetExpensesAmountByDateAsync(
            user.Id,
            null,
            null,
            CancellationToken.None
        );

        result.Should().ContainSingle();
        result
            .Should()
            .Contain(x =>
                x.Amount == 100
                && x.Currency == Currency.NZD
                && x.Date == TestDataFactory.DefaultDate
            );
    }

    [Fact]
    public async Task GetExpensesAmountByDateAsync_ReturnsFilteredExpenseByDate()
    {
        var user = TestDataFactory.CreateUser("a", "b", null, UserRole.User, null);
        var date1 = new DateOnly(2026, 8, 10);
        var expense1 = TestDataFactory.CreateExpense(user.Id, null, null, 100, Currency.NZD, date1);
        var date2 = new DateOnly(2026, 8, 5);
        var expense2 = TestDataFactory.CreateExpense(user.Id, null, null, 200, Currency.NZD, date2);
        var date3 = new DateOnly(2026, 8, 15);
        var expense3 = TestDataFactory.CreateExpense(user.Id, null, null, 300, Currency.RUB, date3);

        _context.Users.Add(user);
        _context.Expenses.Add(expense1);
        _context.Expenses.Add(expense2);
        _context.Expenses.Add(expense3);
        await _context.SaveChangesAsync();

        var result = await _repository.GetExpensesAmountByDateAsync(
            user.Id,
            new DateOnly(2026, 8, 10),
            new DateOnly(2026, 8, 20),
            CancellationToken.None
        );

        result.Should().HaveCount(2);
        result
            .Should()
            .Contain(x => x.Amount == 100 && x.Currency == Currency.NZD && x.Date == date1)
            .And.Contain(x => x.Amount == 300 && x.Currency == Currency.RUB && x.Date == date3);
    }

    [Fact]
    public async Task GetExpensesAmountByShopAsync_ReturnsExpensesByShop()
    {
        var user = TestDataFactory.CreateUser("a", "b", null, UserRole.User, null);
        var shop1 = TestDataFactory.CreateShop("paknsave", null, null);
        var shop2 = TestDataFactory.CreateShop("newworld", null, null);
        var expense1 = TestDataFactory.CreateExpense(
            user.Id,
            null,
            shop1.Id,
            100,
            Currency.NZD,
            null
        );
        var expense2 = TestDataFactory.CreateExpense(
            user.Id,
            null,
            shop1.Id,
            200,
            Currency.NZD,
            null
        );
        var expense3 = TestDataFactory.CreateExpense(
            user.Id,
            null,
            shop2.Id,
            300,
            Currency.NZD,
            null
        );
        var expense4 = TestDataFactory.CreateExpense(user.Id, null, null, 400, Currency.RUB, null);

        _context.Users.Add(user);
        _context.Shops.Add(shop1);
        _context.Shops.Add(shop2);
        _context.Expenses.Add(expense1);
        _context.Expenses.Add(expense2);
        _context.Expenses.Add(expense3);
        _context.Expenses.Add(expense4);
        await _context.SaveChangesAsync();

        var result = await _repository.GetExpensesAmountByShopAsync(
            user.Id,
            null,
            null,
            CancellationToken.None
        );

        result.Should().HaveCount(3);
        result
            .Should()
            .Contain(x =>
                x.ShopId == shop1.Id
                && x.ShopName == shop1.Name
                && x.TotalAmount == 300
                && x.Currency == Currency.NZD
            )
            .And.Contain(x =>
                x.ShopId == shop2.Id
                && x.ShopName == shop2.Name
                && x.TotalAmount == 300
                && x.Currency == Currency.NZD
            )
            .And.Contain(x =>
                x.ShopId == null
                && x.ShopName == "Unknown"
                && x.TotalAmount == 400
                && x.Currency == Currency.RUB
            );
    }

    [Fact]
    public async Task GetGroupedAmountByPeriodAsync_ReturnsGroupedAmountsByWeek()
    {
        // TODO: Add test
    }
}
