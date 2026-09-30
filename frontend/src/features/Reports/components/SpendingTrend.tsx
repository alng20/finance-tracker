import "./css/SpendingTrend.css";

import { useState } from "react";

import {
  formatAmount,
  formatDate,
  getTodayDate,
} from "../../shared/common/utils";
import useGroupedAmount from "../hooks/useGroupedAmount";
import type { ReportGroupingType } from "../types/GetGroupedAmountResponse";
import type { ReportFiltersData } from "../types/ReportFiltersData";
import ReportBarList from "./ReportBarList";

const groupingTypes: ReportGroupingType[] = ["Day", "Week", "Month", "Year"];

type SpendingTrendProps = {
  filters: ReportFiltersData;
  groupingType: ReportGroupingType;
  onGroupingTypeChange: (groupingType: ReportGroupingType) => void;
};

function formatPeriodLabel(fromDate: string, toDate: string): string {
  if (fromDate === toDate) {
    return formatDate(fromDate);
  }
  return `${formatDate(fromDate)} \u2013 ${formatDate(toDate)}`;
}

function formatDateOnly(date: Date): string {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");

  return `${year}-${month}-${day}`;
}

function addDays(date: string, days: number): string {
  const result = new Date(date);
  result.setDate(result.getDate() + days);

  return formatDateOnly(result);
}

function getMonthBounds(
  date: string,
  monthsBack: number,
): { start: string; end: string } {
  const base = new Date(date);
  const year = base.getFullYear();
  const month = base.getMonth() - monthsBack;

  return {
    start: formatDateOnly(new Date(year, month, 1)),
    end: formatDateOnly(new Date(year, month + 1, 0)),
  };
}

function getEffectiveTrendFilters(
  filters: ReportFiltersData,
  groupingType: ReportGroupingType,
  pageOffset: number,
): {
  filters: ReportFiltersData;
  isClamped: boolean;
  hasEarlierFilterRoom: boolean;
} {
  if (groupingType !== "Day") {
    return { filters, isClamped: false, hasEarlierFilterRoom: false };
  }

  const anchorDate = filters.toDate ?? getTodayDate();
  const { start: monthStart, end: monthEnd } = getMonthBounds(
    anchorDate,
    pageOffset,
  );

  const toDate =
    pageOffset === 0 && anchorDate < monthEnd ? anchorDate : monthEnd;
  const fromDate =
    filters.fromDate && filters.fromDate > monthStart
      ? filters.fromDate
      : monthStart;

  const isClamped = filters.toDate !== toDate || filters.fromDate !== fromDate;
  const dayBeforeWindow = addDays(fromDate, -1);
  const hasEarlierFilterRoom =
    !filters.fromDate || dayBeforeWindow >= filters.fromDate;

  return {
    filters: { ...filters, fromDate, toDate },
    isClamped,
    hasEarlierFilterRoom,
  };
}

function SpendingTrend({
  filters,
  groupingType,
  onGroupingTypeChange,
}: SpendingTrendProps) {
  const [pageOffset, setPageOffset] = useState(0);

  // Reset paging back to the most recent window whenever the shared
  // filters or grouping type change, without a separate reset effect.
  const filtersKey = `${filters.currency}|${filters.fromDate ?? ""}|${filters.toDate ?? ""}|${groupingType}`;
  const [lastFiltersKey, setLastFiltersKey] = useState(filtersKey);
  if (filtersKey !== lastFiltersKey) {
    setLastFiltersKey(filtersKey);
    setPageOffset(0);
  }

  const {
    filters: effectiveFilters,
    isClamped,
    hasEarlierFilterRoom,
  } = getEffectiveTrendFilters(filters, groupingType, pageOffset);
  const { groupedAmount, isLoading, error } = useGroupedAmount(
    effectiveFilters,
    groupingType,
  );

  const { filters: earlierFilters } = getEffectiveTrendFilters(
    filters,
    groupingType,
    pageOffset + 1,
  );
  const { groupedAmount: earlierGroupedAmount, isLoading: isEarlierLoading } =
    useGroupedAmount(
      hasEarlierFilterRoom ? earlierFilters : effectiveFilters,
      groupingType,
    );
  const canGoOlder =
    hasEarlierFilterRoom &&
    !isEarlierLoading &&
    (earlierGroupedAmount?.amountByPeriod.length ?? 0) > 0;

  const items = (groupedAmount?.amountByPeriod ?? []).map((period) => ({
    key: `${period.reportPeriod.fromDate}-${period.reportPeriod.toDate}`,
    label: formatPeriodLabel(
      period.reportPeriod.fromDate,
      period.reportPeriod.toDate,
    ),
    amount: period.amount,
  }));

  return (
    <section className="spending-trend" aria-label="Spending trend">
      <div className="spending-trend__header">
        <h2>Spending trend</h2>
        <label className="spending-trend__grouping">
          Group by
          <select
            value={groupingType}
            onChange={(event) =>
              onGroupingTypeChange(event.target.value as ReportGroupingType)
            }
          >
            {groupingTypes.map((type) => (
              <option key={type} value={type}>
                {type}
              </option>
            ))}
          </select>
        </label>
      </div>

      {groupingType === "Day" && (
        <div className="spending-trend__pager">
          <p className="spending-trend__note">
            Day view shows one calendar month at a time
            {isClamped
              ? ` (${formatPeriodLabel(effectiveFilters.fromDate ?? "", effectiveFilters.toDate ?? "")} shown)`
              : ""}
            .
          </p>
          <div className="spending-trend__pager-buttons">
            {canGoOlder && (
              <button
                type="button"
                onClick={() => setPageOffset((offset) => offset + 1)}
              >
                &larr; Previous month
              </button>
            )}
            <button
              type="button"
              disabled={pageOffset === 0}
              onClick={() => setPageOffset((offset) => Math.max(0, offset - 1))}
            >
              Next month &rarr;
            </button>
          </div>
        </div>
      )}

      {isLoading && <div>Loading...</div>}
      {error && <div>Failed to load spending trend.</div>}
      {!isLoading && !error && (
        <ReportBarList
          items={items}
          amountFormatter={(amount) => formatAmount(amount, filters.currency)}
          emptyMessage="No spending recorded for this period."
        />
      )}
    </section>
  );
}

export default SpendingTrend;
