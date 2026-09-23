import "./css/ExpenseResultsSummary.css";

import { formatAmount } from "../../../shared/common/utils";
import type {
  ExpenseDto,
  ExpenseMetadata,
} from "../../types/GetExpensesResponse";

type ExpenseResultsSummaryProps = {
  expenses: ExpenseDto[];
  expensesMetadata: ExpenseMetadata | null;
  totalCount: number;
};

export function ExpenseResultsSummary({
  expensesMetadata,
  totalCount,
}: ExpenseResultsSummaryProps) {
  return (
    <section className="expense-results-summary" aria-label="Expense summary">
      <div className="expense-results-summary__heading">
        <div>
          <p className="expense-results-summary__eyebrow">Spending overview</p>
          <h2>Spending snapshot</h2>
        </div>
        <span className="expense-results-summary__count">
          {totalCount} {totalCount === 1 ? "expense" : "expenses"}
        </span>
      </div>

      <div className="expense-results-summary__metrics">
        <div className="expense-results-summary__metric">
          {/* TODO: Show all expenses amount */}
          <span>Summary</span>
          <strong>
            {expensesMetadata
              ? formatAmount(
                  expensesMetadata.summaryAmount,
                  expensesMetadata.currency,
                )
              : "Loading.."}
          </strong>
        </div>
        <div className="expense-results-summary__metric">
          <span>Average per expense</span>
          <strong>
            {expensesMetadata
              ? formatAmount(
                  expensesMetadata.summaryAmount / totalCount,
                  expensesMetadata.currency,
                )
              : "Loading.."}
          </strong>
        </div>
        <div className="expense-results-summary__metric">
          {/* TODO: Show some statistic */}
          <span>Total expenses</span>
          <strong>{totalCount}</strong>
        </div>
      </div>
    </section>
  );
}
