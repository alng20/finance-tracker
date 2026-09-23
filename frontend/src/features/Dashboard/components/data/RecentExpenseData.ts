import type { ExpenseDto } from "../../../Expenses/types/GetExpensesResponse";
import type { Currency } from "../../../shared/types/Currency";

export type RecentExpenseData = {
  id: string;
  shopName?: string;
  amount: number;
  currency: Currency;
  expenseDate: string;
};

export function mapRecentExpense(expense: ExpenseDto): RecentExpenseData {
  return {
    id: expense.id,
    shopName: expense?.shopName ?? "Unknown Shop",
    amount: expense.totalAmount,
    currency: expense.currency,
    expenseDate: expense.expenseDate,
  };
}
