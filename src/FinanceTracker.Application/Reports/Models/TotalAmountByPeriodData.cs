using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Reports.Models;

public record TotalAmountByPeriodData(
    ReportPeriod ReportPeriod,
    decimal Amount,
    Currency Currency
);
