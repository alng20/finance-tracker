using FinanceTracker.Application.Reports.Enums;
using FinanceTracker.Application.Reports.Models;

namespace FinanceTracker.Application.Common.Interfaces.Persistence;

public interface IExpenseReportRepository
{
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
    Task<IReadOnlyCollection<GroupedTotalByPeriodData>> GetGroupedAmountByPeriodAsync(
        Guid userId,
        DateOnly? fromDate,
        DateOnly? toDate,
        ReportGroupingType groupingType,
        CancellationToken cancellationToken
    );
}
