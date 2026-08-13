using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Reports.Models;

public record GroupedTotalByPeriodData(
    ReportPeriod ReportPeriod,
    decimal Amount,
    Currency Currency
);
