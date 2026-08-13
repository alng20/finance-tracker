using FinanceTracker.Application.Reports.Enums;

namespace FinanceTracker.Application.Reports.DTOs;

public record GetGroupedAmountByPeriodResultDto(
    ReportGroupingType GroupingType,
    IReadOnlyCollection<PeriodAmountReportDto> AmountByPeriod
);
