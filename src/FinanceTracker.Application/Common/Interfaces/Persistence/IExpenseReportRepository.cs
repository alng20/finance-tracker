using FinanceTracker.Application.Reports.Enums;
using FinanceTracker.Application.Reports.Models;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IExpenseReportRepository
{
    Task<IReadOnlyCollection<ExpenseAmountByDateData>> GetTotalAmountAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    );

    Task<IReadOnlyCollection<ExpensesTotalWithDetailsData>> GetExpensesTotalWithDetailsAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    );
    Task<IReadOnlyCollection<ExpenseAmountByDateData>> GetExpensesAmountByDateAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    );
    Task<IReadOnlyCollection<ExpenseShopAmountData>> GetExpensesAmountByShopAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    );
    Task<IReadOnlyCollection<ExpenseRetailerAmountData>> GetExpensesAmountByRetailerAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        CancellationToken cancellationToken
    );
    Task<IReadOnlyCollection<TotalAmountByPeriodData>> GetGroupedAmountByPeriodAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        ReportGroupingType groupingType,
        CancellationToken cancellationToken
    );
}
