using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Reports.DTOs;

public record GetTotalAmountForPeriodResultDto(
    DateOnly? FromDate,
    DateOnly? ToDate,
    decimal TotalAmount,
    Currency Currency
);
