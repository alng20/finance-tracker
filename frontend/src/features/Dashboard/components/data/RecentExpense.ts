import type { ExpenseDto } from "../../../Expenses/types/GetExpensesResponse";
import { formatAmount } from "../../../shared/common/utils";

export type RecentExpense = {
  id: string;
  shopName?: string;
  amountFormatted: string;
  expenseDate: string;
};

export function mapRecentExpense(expense: ExpenseDto): RecentExpense {
  return {
    id: expense.id,
    shopName: expense?.shopName ?? "Unknown Shop",
    amountFormatted: formatAmount(expense.totalAmount, expense.currency),
    expenseDate: expense.expenseDate,
  };
}
