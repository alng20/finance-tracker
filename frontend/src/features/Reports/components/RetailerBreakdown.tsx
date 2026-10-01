import "./css/RetailerBreakdown.css";

import { formatAmount } from "../../shared/common/utils";
import * as reportsApi from "../api/reportsApi";
import useReportAmount from "../hooks/useReportAmount";
import type { ReportFiltersData } from "../types/ReportFiltersData";
import type { ReportBarListItem } from "./ReportBarList";
import ReportBarList from "./ReportBarList";

const TOP_N = 8;

type RetailerBreakdownProps = {
  filters: ReportFiltersData;
};

function RetailerBreakdown({ filters }: RetailerBreakdownProps) {
  const {
    result: retailerAmount,
    isLoading,
    error,
  } = useReportAmount(filters, reportsApi.getRetailerAmount);

  const sorted = (retailerAmount?.amounts ?? []).map((retailer) => ({
    key: retailer.retailerId ?? "unspecified",
    label: retailer.retailerId ? retailer.retailerName : "Unspecified",
    amount: retailer.totalAmount,
  }));
  // TODO: Add different sorting type
  // .sort((l, r) => r.amount - l.amount);

  const items: ReportBarListItem[] = sorted.slice(0, TOP_N);
  const rest = sorted.slice(TOP_N);
  if (rest.length > 0) {
    items.push({
      key: "other",
      label: `Other (${rest.length})`,
      amount: rest.reduce((sum, item) => sum + item.amount, 0),
    });
  }

  return (
    <section className="retailer-breakdown" aria-label="Spending by retailer">
      <h2>By retailer</h2>

      {isLoading && <div>Loading...</div>}
      {error && <div>Failed to load spending by retailer.</div>}
      {!isLoading && !error && (
        <ReportBarList
          items={items}
          amountFormatter={(amount) => formatAmount(amount, filters.currency)}
          emptyMessage="No spending recorded by retailer for this period."
        />
      )}
    </section>
  );
}

export default RetailerBreakdown;
