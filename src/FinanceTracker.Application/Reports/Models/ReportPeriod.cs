using System.Globalization;

namespace FinanceTracker.Application.Reports.Models;

public record ReportPeriod(DateOnly? FromDate, DateOnly? ToDate)
{
    public static ReportPeriod GetWeekPeriod(int year, int week)
    {
        var from = ISOWeek.ToDateOnly(year, week, DayOfWeek.Monday);
        var to = from.AddDays(6);

        return new ReportPeriod(from, to);
    }
    public static ReportPeriod GetMonthPeriod(int year, int month)
    {
        var from = new DateOnly(year, month, 1);
        var to = from.AddMonths(1).AddDays(-1);

        return new ReportPeriod(from, to);
    }

    public static ReportPeriod GetYearPeriod(int year)
    {
        var from = new DateOnly(year, 1, 1);
        var to = from.AddYears(1).AddDays(-1);

        return new ReportPeriod(from, to);
    }
}
