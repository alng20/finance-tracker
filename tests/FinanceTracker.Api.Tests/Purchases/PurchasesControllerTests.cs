using System.Net;
using System.Net.Http.Json;

using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;

using FluentAssertions;

namespace FinanceTracker.Api.Tests;

public class PurchasesControllerTests : IClassFixture<FinanceTrackerApiFactory>
{
    private readonly FinanceTrackerApiFactory _factory;

    public PurchasesControllerTests(FinanceTrackerApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetPurchases_ReturnsOkWithPagedResult()
    {
        using var client = _factory.CreateUserClient();

        var response = await client.GetAsync("/api/purchases?currency=NZD&page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<GetItemPurchasesResultDto>>();
        result.Should().NotBeNull();
        result!.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPricesById_ReturnsOkForEachSortType()
    {
        using var client = _factory.CreateUserClient();

        foreach (var sortType in new[] { "DateDesc", "DateAsc", "PriceDesc", "PriceAsc" })
        {
            var response = await client.GetAsync(
                $"/api/purchases/{Guid.NewGuid()}?currency=NZD&sortType={sortType}"
            );

            response.StatusCode.Should().Be(HttpStatusCode.OK, sortType);
        }
    }

    [Fact]
    public async Task GetPricesById_InvalidSortType_ReturnsBadRequest()
    {
        using var client = _factory.CreateUserClient();

        var response = await client.GetAsync(
            $"/api/purchases/{Guid.NewGuid()}?currency=NZD&sortType=Nope"
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
