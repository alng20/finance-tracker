import "./css/ShopBreakdown.css";

import { useState } from "react";

import { formatAmount } from "../../shared/common/utils";
import * as reportsApi from "../api/reportsApi";
import useReportAmount from "../hooks/useReportAmount";
import type { ReportFiltersData } from "../types/ReportFiltersData";
import ReportBarList, { type ReportBarListItem } from "./ReportBarList";

const TOP_N = 8;

type ShopBreakdownProps = {
  filters: ReportFiltersData;
};

function ShopBreakdown({ filters }: ShopBreakdownProps) {
  const {
    result: shopAmount,
    isLoading,
    error,
  } = useReportAmount(filters, reportsApi.getShopAmount);
  const [isOtherExpanded, setIsOtherExpanded] = useState(false);

  // Collapse "Other" back to its aggregate whenever the filters change,
  // without introducing a separate effect (React's recommended pattern
  // for resetting state in response to prop changes during render).
  const filtersKey = `${filters.currency}|${filters.fromDate ?? ""}|${filters.toDate ?? ""}`;
  const [lastFiltersKey, setLastFiltersKey] = useState(filtersKey);
  if (filtersKey !== lastFiltersKey) {
    setLastFiltersKey(filtersKey);
    setIsOtherExpanded(false);
  }

  const sorted = (shopAmount?.amounts ?? []).map((shop) => ({
    key: shop.shopId ?? "unspecified",
    label: shop.shopId ? shop.shopName : "Unspecified",
    amount: shop.totalAmount,
  }));
  // TODO: Add different sorting type
  // .sort((l, r) => r.amount - l.amount);

  const rest = sorted.slice(TOP_N);
  const items: ReportBarListItem[] = isOtherExpanded
    ? sorted
    : sorted.slice(0, TOP_N);

  if (!isOtherExpanded && rest.length > 0) {
    items.push({
      key: "other",
      label: `Other (${rest.length})`,
      amount: rest.reduce((sum, item) => sum + item.amount, 0),
      onClick: () => setIsOtherExpanded(true),
    });
  }

  return (
    <section className="shop-breakdown" aria-label="Spending by shop">
      <h2>By shop</h2>

      {isLoading && <div>Loading...</div>}
      {error && <div>Failed to load spending by shop.</div>}
      {!isLoading && !error && (
        <ReportBarList
          items={items}
          amountFormatter={(amount) => formatAmount(amount, filters.currency)}
          emptyMessage="No spending recorded by shop for this period."
        />
      )}
      {isOtherExpanded && rest.length > 0 && (
        <button
          type="button"
          className="shop-breakdown__toggle"
          onClick={() => setIsOtherExpanded(false)}
        >
          Show less
        </button>
      )}
    </section>
  );
}

export default ShopBreakdown;
