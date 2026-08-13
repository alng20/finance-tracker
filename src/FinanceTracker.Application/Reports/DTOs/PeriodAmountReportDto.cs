using FinanceTracker.Application.Reports.Models;

namespace FinanceTracker.Application.Reports.DTOs;

public record PeriodAmountReportDto(ReportPeriod ReportPeriod, decimal Amount);
