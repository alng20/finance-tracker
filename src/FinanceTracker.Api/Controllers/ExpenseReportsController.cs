using FinanceTracker.Application.Reports.DTOs;
using FinanceTracker.Application.Reports.Enums;
using FinanceTracker.Application.Reports.Queries.GetCategoryAmountForPeriod;
using FinanceTracker.Application.Reports.Queries.GetGroupedAmountByPeriod;
using FinanceTracker.Application.Reports.Queries.GetRetailerAmountForPeriod;
using FinanceTracker.Application.Reports.Queries.GetShopAmountForPeriod;
using FinanceTracker.Application.Reports.Queries.GetTotalAmountForPeriod;
using FinanceTracker.Domain.Enums;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/reports")]
public class ExpenseReportController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("total")]
    public async Task<ActionResult<GetTotalAmountForPeriodResultDto>> GetTotalAmount(
        [FromQuery] Currency currency,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default
    )
    {
        var report = await _mediator.Send(
            new GetTotalAmountForPeriodQuery(fromDate, toDate, currency),
            cancellationToken
        );
        return Ok(report);
    }

    // TODO: Add category filters
    [Authorize]
    [HttpGet("by_category")]
    public async Task<ActionResult<GetCategoryAmountForPeriodResultDto>> GetAmountByCategory(
        [FromQuery] Currency currency,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default
    )
    {
        var report = await _mediator.Send(
            new GetCategoryAmountForPeriodQuery(fromDate, toDate, currency),
            cancellationToken
        );
        return Ok(report);
    }

    [Authorize]
    [HttpGet("grouped")]
    public async Task<ActionResult<GetGroupedAmountByPeriodResultDto>> GetGroupedAmount(
        [FromQuery] Currency currency,
        [FromQuery] ReportGroupingType groupingType,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default
    )
    {
        var report = await _mediator.Send(
            new GetGroupedAmountByPeriodQuery(fromDate, toDate, groupingType, currency),
            cancellationToken
        );
        return Ok(report);
    }

    // TODO: Add shop filters
    [Authorize]
    [HttpGet("by_shop")]
    public async Task<ActionResult<GetShopAmountForPeriodResultDto>> GetAmountByShop(
        [FromQuery] Currency currency,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default
    )
    {
        var report = await _mediator.Send(
            new GetShopAmountForPeriodQuery(fromDate, toDate, currency),
            cancellationToken
        );
        return Ok(report);
    }

    // TODO: Add retailer filters
    [Authorize]
    [HttpGet("by_retailer")]
    public async Task<ActionResult<GetRetailerAmountForPeriodResultDto>> GetAmountByRetailer(
        [FromQuery] Currency currency,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default
    )
    {
        var report = await _mediator.Send(
            new GetRetailerAmountForPeriodQuery(fromDate, toDate, currency),
            cancellationToken
        );
        return Ok(report);
    }

    [Authorize]
    [HttpGet("by_retailers_shop")]
    public async Task<ActionResult> GetShopAmountByRetailer()
    {
        // TODO: Implement
        return NotFound("Not implemented yet");
    }
}
