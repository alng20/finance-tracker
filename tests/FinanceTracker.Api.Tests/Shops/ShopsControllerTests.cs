using System.Net;
using System.Net.Http.Json;

using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Shops.Commands.CreateShop;
using FinanceTracker.Application.Shops.DTOs;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.TestHelpers;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Api.Tests;

public class ShopControllerTests : IClassFixture<FinanceTrackerApiFactory>
{
    private readonly FinanceTrackerApiFactory _factory = null!;

    public ShopControllerTests(FinanceTrackerApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetShops_ReturnsOk()
    {
        await _factory.ClearShopsAsync();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();

        var shop1 = TestDataFactory.CreateShop("paknsave", null, null);
        var shop2 = TestDataFactory.CreateShop("newworld", null, null);
        context.Shops.Add(shop1);
        context.Shops.Add(shop2);
        await context.SaveChangesAsync();

        using var client = _factory.CreateUserClient();
        var response = await client.GetAsync("/api/shops");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var shops = await response.Content.ReadFromJsonAsync<PagedResult<ShopDto>>();
        shops.Should().NotBeNull();
        shops.Page.Should().Be(1);
        shops.PageSize.Should().Be(20);
        shops.TotalCount.Should().Be(2);
        shops.TotalPages.Should().Be(1);
        shops.HasNextPage.Should().Be(false);
        shops.HasPreviousPage.Should().Be(false);
        shops.Data.Should().HaveCount(2);
        shops
            .Data.Should()
            .Contain(x => x.Name == "paknsave")
            .And.Contain(x => x.Name == "newworld");
    }

    [Fact]
    public async Task PostShop_Admin_ReturnsOk()
    {
        await _factory.ClearShopsAsync();

        using var client = _factory.CreateAdminClient();
        var cmd = new CreateShopCommand("paknsave", null, "New Zealand", "Wellington");
        var response = await client.PostAsJsonAsync("/api/shops", cmd);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var shop = await response.Content.ReadFromJsonAsync<ShopDto>();
        shop.Should().NotBeNull();
        shop!.Name.Should().Be("paknsave");
        shop.RetailerId.Should().BeNull();
        shop.Country.Should().Be("New Zealand");
        shop.City.Should().Be("Wellington");

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();
        var dbShop = await context.Shops.SingleAsync(x => x.Id == shop.Id);
        dbShop.Name.Should().Be("paknsave");
    }

    [Fact]
    public async Task DeleteShop_User_ReturnsForbidden()
    {
        await _factory.ClearShopsAsync();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();

        var shop = TestDataFactory.CreateShop("woolworths", null, null);
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        using var client = _factory.CreateUserClient();
        var response = await client.DeleteAsync($"/api/shops/{shop.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteShop_Admin_ReturnsNoContent()
    {
        await _factory.ClearShopsAsync();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();

        var shop = TestDataFactory.CreateShop("woolworths", null, null);
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        using var client = _factory.CreateAdminClient();
        var response = await client.DeleteAsync($"/api/shops/{shop.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteShop_Admin_ReturnsNotFound()
    {
        await _factory.ClearShopsAsync();

        var shopId = Guid.NewGuid();

        using var client = _factory.CreateAdminClient();
        var response = await client.DeleteAsync($"/api/shops/{shopId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
