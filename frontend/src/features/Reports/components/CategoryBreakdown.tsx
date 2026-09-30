import "./css/CategoryBreakdown.css";

import { formatAmount } from "../../shared/common/utils";
import * as reportsApi from "../api/reportsApi";
import useReportAmount from "../hooks/useReportAmount";
import type { ReportFiltersData } from "../types/ReportFiltersData";
import ReportBarList from "./ReportBarList";

type CategoryBreakdownProps = {
  filters: ReportFiltersData;
};

function CategoryBreakdown({ filters }: CategoryBreakdownProps) {
  const {
    result: categoryAmount,
    isLoading,
    error,
  } = useReportAmount(filters, reportsApi.getCategoryAmount);

  const items = (categoryAmount?.amounts ?? []).map((category) => ({
    key: category.categoryId,
    label: category.categoryName,
    amount: category.totalAmount,
  }));
  // TODO: Add different sorting type
  // .sort((l, r) => r.amount - l.amount);

  return (
    <section className="category-breakdown" aria-label="Spending by category">
      <h2>By category</h2>

      {isLoading && <div>Loading...</div>}
      {error && <div>Failed to load spending by category.</div>}
      {!isLoading && !error && (
        <>
          <ReportBarList
            items={items}
            amountFormatter={(amount) => formatAmount(amount, filters.currency)}
            emptyMessage="No categorized spending for this period."
          />
          {categoryAmount && (
            <p className="category-breakdown__callout">
              Detailed:{" "}
              {formatAmount(categoryAmount.detailedAmount, filters.currency)}
              {" \u00b7 "}
              Undetailed:{" "}
              {formatAmount(categoryAmount.undetailedAmount, filters.currency)}
            </p>
          )}
        </>
      )}
    </section>
  );
}

export default CategoryBreakdown;
