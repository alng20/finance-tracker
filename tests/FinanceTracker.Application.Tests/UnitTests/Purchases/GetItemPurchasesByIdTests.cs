using FinanceTracker.Application.Common.Interfaces.Providers;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Purchases.DTOs;
using FinanceTracker.Application.Purchases.Enums;
using FinanceTracker.Application.Purchases.Queries.GetItemPurchasesById;
using FinanceTracker.Domain.Enums;

using FluentAssertions;

using Moq;

namespace FinanceTracker.Application.Tests.UnitTests.Purchases;

public class GetItemPurchasesByIdTests
{
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _itemId = Guid.NewGuid();

    private readonly Mock<ICurrentUserService> _currentUserMock = new();
    private readonly Mock<IPurchaseProvider> _providerMock = new();

    [Fact]
    public async Task Handle_PassesQueryAndCurrentUserToProvider()
    {
        _currentUserMock.Setup(x => x.UserId).Returns(_userId);
        var from = new DateOnly(2026, 1, 1);
        var to = new DateOnly(2026, 2, 1);
        var expected = new PagedResult<GetItemPurchasesByIdResultDto>(
            [new GetItemPurchasesByIdResultDto(2m, Currency.NZD, from, null, "Unknown")],
            2,
            10,
            11
        );

        _providerMock
            .Setup(x =>
                x.GetPricesByIdAsync(
                    _itemId,
                    _userId,
                    2,
                    10,
                    from,
                    to,
                    PricesSortType.PriceAsc,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(expected);

        var handler = new GetItemPurchasesByIdHandler(
            _currentUserMock.Object,
            _providerMock.Object
        );

        var result = await handler.Handle(
            new GetItemPurchasesByIdQuery(
                _itemId,
                Currency.NZD,
                2,
                10,
                from,
                to,
                PricesSortType.PriceAsc
            ),
            CancellationToken.None
        );

        result.Should().BeSameAs(expected);
    }

    [Fact]
    public async Task Validator_DefaultQuery_IsValid()
    {
        var result = await new GetItemPurchasesByIdValidator().ValidateAsync(
            new GetItemPurchasesByIdQuery(_itemId, Currency.NZD)
        );

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_InvalidSortType_IsInvalid()
    {
        var query = new GetItemPurchasesByIdQuery(
            _itemId,
            Currency.NZD,
            SortType: (PricesSortType)99
        );

        var result = await new GetItemPurchasesByIdValidator().ValidateAsync(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(query.SortType));
    }

    [Fact]
    public async Task Validator_PageSizeAboveMax_IsInvalid()
    {
        var result = await new GetItemPurchasesByIdValidator().ValidateAsync(
            new GetItemPurchasesByIdQuery(_itemId, Currency.NZD, PageSize: 501)
        );

        result.IsValid.Should().BeFalse();
    }
}
