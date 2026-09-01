using FinanceTracker.Application.Reports.Enums;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Reports.DTOs;

public record GetGroupedAmountByPeriodResultDto(
    Currency Currency,
    ReportGroupingType GroupingType,
    IReadOnlyCollection<PeriodAmountReportDto> AmountByPeriod
);
