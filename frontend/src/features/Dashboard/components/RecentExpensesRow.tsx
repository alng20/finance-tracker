import "./css/RecentExpensesRow.css";

import { formatAmount, formatDate } from "../../shared/common/utils";
import type { RecentExpenseData } from "./data/RecentExpenseData";

type RecentExpenseRowProps = {
  expense: RecentExpenseData;
};

function RecentExpenseRow({ expense }: RecentExpenseRowProps) {
  return (
    <div className="recent_expenses_row">
      <div className="recent_expenses_row__first_line">
        <span className="recent_expenses_row__date">
          {formatDate(expense.expenseDate)}
        </span>
      </div>
      <div className="recent_expenses_row__second_line">
        <span className="recent_expenses_row__amount">
          {formatAmount(expense.amount, expense.currency)}
        </span>
        <span className="recent_expenses_row__shop">{expense.shopName}</span>
      </div>
    </div>
  );
}

export default RecentExpenseRow;
