using FinanceTracker.Application.Purchases.Enums;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.ValueObjects;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Persistence.Providers;
using FinanceTracker.TestHelpers;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Testcontainers.PostgreSql;

namespace FinanceTracker.Infrastructure.Tests.IntegrationTests.Persistence;

public class PurchaseProviderTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .WithDatabase("finance_tracker_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private FinanceTrackerDbContext _context = null!;
    private PurchaseProvider _provider = null!;

    private User _user = null!;
    private Item _item = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _context = new FinanceTrackerDbContext(options);
        await _context.Database.MigrateAsync();

        _provider = new PurchaseProvider(_context);

        _user = TestDataFactory.CreateUser("a", "b", null, UserRole.User, null);
        var category = TestDataFactory.CreateItemCategory("Food");
        _item = TestDataFactory.CreateItem("Milk", category.Id);

        _context.Users.Add(_user);
        _context.ItemCategories.Add(category);
        _context.Items.Add(_item);
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    private async Task AddPurchaseAsync(Guid userId, Guid itemId, decimal price, DateOnly date)
    {
        var expense = TestDataFactory.CreateExpense(
            userId,
            null,
            null,
            price,
            Currency.NZD,
            date
        );
        expense.AddDetail(itemId, Money.Create(price, Currency.NZD), 1, 0);

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
    }

    private async Task SeedPricesAsync()
    {
        await AddPurchaseAsync(_user.Id, _item.Id, 3m, new DateOnly(2026, 3, 1));
        await AddPurchaseAsync(_user.Id, _item.Id, 1m, new DateOnly(2026, 1, 1));
        await AddPurchaseAsync(_user.Id, _item.Id, 2m, new DateOnly(2026, 2, 1));
    }

    private Task<Application.Common.Models.PagedResult<Application.Purchases.DTOs.GetItemPurchasesByIdResultDto>> GetPricesAsync(
        PricesSortType sortType,
        int page = 1,
        int pageSize = 20,
        DateOnly? from = null,
        DateOnly? to = null
    ) =>
        _provider.GetPricesByIdAsync(
            _item.Id,
            _user.Id,
            page,
            pageSize,
            from,
            to,
            sortType,
            CancellationToken.None
        );

    [Theory]
    [InlineData(PricesSortType.DateDesc, new[] { 3, 2, 1 })]
    [InlineData(PricesSortType.DateAsc, new[] { 1, 2, 3 })]
    [InlineData(PricesSortType.PriceDesc, new[] { 3, 2, 1 })]
    [InlineData(PricesSortType.PriceAsc, new[] { 1, 2, 3 })]
    public async Task GetPricesByIdAsync_SortsByRequestedType(
        PricesSortType sortType,
        int[] expectedPrices
    )
    {
        await SeedPricesAsync();

        var result = await GetPricesAsync(sortType);

        result.Data.Select(x => (int)x.Price).Should().Equal(expectedPrices);
    }

    [Fact]
    public async Task GetPricesByIdAsync_PriceSort_IsAppliedBeforePaging()
    {
        await SeedPricesAsync();

        var result = await GetPricesAsync(PricesSortType.PriceAsc, page: 2, pageSize: 2);

        result.TotalCount.Should().Be(3);
        result.Data.Select(x => (int)x.Price).Should().Equal(3);
    }

    [Fact]
    public async Task GetPricesByIdAsync_FiltersByPeriod()
    {
        await SeedPricesAsync();

        var result = await GetPricesAsync(
            PricesSortType.DateAsc,
            from: new DateOnly(2026, 2, 1),
            to: new DateOnly(2026, 3, 1)
        );

        result.Data.Select(x => (int)x.Price).Should().Equal(2, 3);
    }

    [Fact]
    public async Task GetPricesByIdAsync_ReturnsOnlyCurrentUserPrices()
    {
        var other = TestDataFactory.CreateUser("c", "d", null, UserRole.User, null);
        _context.Users.Add(other);
        await _context.SaveChangesAsync();

        await AddPurchaseAsync(_user.Id, _item.Id, 1m, new DateOnly(2026, 1, 1));
        await AddPurchaseAsync(other.Id, _item.Id, 9m, new DateOnly(2026, 1, 2));

        var result = await GetPricesAsync(PricesSortType.DateDesc);

        result.Data.Should().ContainSingle().Which.Price.Should().Be(1m);
    }
}
