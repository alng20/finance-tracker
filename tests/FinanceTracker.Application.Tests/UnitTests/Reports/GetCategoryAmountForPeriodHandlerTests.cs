using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Reports.DTOs;
using FinanceTracker.Application.Reports.Models;
using FinanceTracker.Application.Reports.Queries.GetCategoryAmountForPeriod;
using FinanceTracker.Domain.Enums;
using FluentAssertions;
using Moq;

namespace FinanceTracker.Application.Tests.UnitTests.Reports;

public class GetCategoryAmountForPeriodHandlerTests
{
    private readonly Guid _userId = Guid.NewGuid();

    private readonly string _foodCategoryName = "Food";
    private readonly Guid _foodCategoryId = Guid.NewGuid();

    private readonly string _transportCategoryName = "Transport";
    private readonly Guid _transportCategoryId = Guid.NewGuid();

    private readonly Mock<ICurrentUserService> _currentUserMock = new();
    private readonly Mock<ICurrencyConverter> _currencyConverterMock = new();
    private readonly Mock<IExpenseReportRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_ReturnsCorrectAmounts_Single()
    {
        _currentUserMock.Setup(x => x.UserId).Returns(_userId);

        var expenses = new[]
        {
            new ExpensesTotalWithDetailsData(
                100m,
                Currency.NZD,
                new[]
                {
                    new ExpenseDetailData(_foodCategoryId, _foodCategoryName, 30m, Currency.NZD),
                }
            ),
        };

        _repositoryMock
            .Setup(x =>
                x.GetExpensesTotalWithDetailsAsync(
                    _userId,
                    null,
                    null,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(expenses);

        _currencyConverterMock
            .Setup(x => x.Convert(It.IsAny<decimal>(), It.IsAny<Currency>(), Currency.NZD))
            .Returns((decimal amount, Currency _, Currency _) => amount);

        var handler = new GetCategoryAmountForPeriodHandler(
            _currentUserMock.Object,
            _currencyConverterMock.Object,
            _repositoryMock.Object
        );

        var query = new GetCategoryAmountForPeriodQuery(null, null, Currency.NZD);
        var result = await handler.Handle(query, CancellationToken.None);

        result.TotalAmount.Should().Be(100m);
        result.DetailedAmount.Should().Be(30m);
        result.UndetailedAmount.Should().Be(70m);
        result.Amounts.Should().ContainSingle();

        var category = result.Amounts.Single();
        category.CategoryId.Should().Be(_foodCategoryId);
        category.CategoryName.Should().Be(_foodCategoryName);
        category.TotalAmount.Should().Be(30m);
    }

    [Fact]
    public async Task Handle_ReturnsCorrectAmounts_Multiple()
    {
        _currentUserMock.Setup(x => x.UserId).Returns(_userId);

        var expenses = new[]
        {
            new ExpensesTotalWithDetailsData(
                100m,
                Currency.NZD,
                new[]
                {
                    new ExpenseDetailData(_foodCategoryId, _foodCategoryName, 30m, Currency.NZD),
                    new ExpenseDetailData(_foodCategoryId, _foodCategoryName, 50m, Currency.NZD),
                    new ExpenseDetailData(
                        _transportCategoryId,
                        _transportCategoryName,
                        10m,
                        Currency.NZD
                    ),
                }
            ),
        };

        _repositoryMock
            .Setup(x =>
                x.GetExpensesTotalWithDetailsAsync(
                    _userId,
                    null,
                    null,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(expenses);

        _currencyConverterMock
            .Setup(x => x.Convert(It.IsAny<decimal>(), It.IsAny<Currency>(), Currency.NZD))
            .Returns((decimal amount, Currency _, Currency _) => amount);

        var handler = new GetCategoryAmountForPeriodHandler(
            _currentUserMock.Object,
            _currencyConverterMock.Object,
            _repositoryMock.Object
        );

        var query = new GetCategoryAmountForPeriodQuery(null, null, Currency.NZD);
        var result = await handler.Handle(query, CancellationToken.None);

        result.TotalAmount.Should().Be(100m);
        result.DetailedAmount.Should().Be(90m);
        result.UndetailedAmount.Should().Be(10m);

        result.Amounts.Should().HaveCount(2);
        // variant 1
        result
            .Amounts.Should()
            .BeEquivalentTo([
                new CategoryAmountDto(_foodCategoryId, _foodCategoryName, 80m),
                new CategoryAmountDto(_transportCategoryId, _transportCategoryName, 10m),
            ]);
        // variant 2
        result
            .Amounts.Should()
            .Contain(x =>
                x.CategoryId == _foodCategoryId
                && x.CategoryName == _foodCategoryName
                && x.TotalAmount == 80m
            );
        result
            .Amounts.Should()
            .Contain(x =>
                x.CategoryId == _transportCategoryId
                && x.CategoryName == _transportCategoryName
                && x.TotalAmount == 10m
            );
        // variant 3
        result
            .Amounts.Should()
            .Contain(x =>
                x.CategoryId == _foodCategoryId
                && x.CategoryName == _foodCategoryName
                && x.TotalAmount == 80m
            )
            .And.Contain(x =>
                x.CategoryId == _transportCategoryId
                && x.CategoryName == _transportCategoryName
                && x.TotalAmount == 10m
            );
    }
}
