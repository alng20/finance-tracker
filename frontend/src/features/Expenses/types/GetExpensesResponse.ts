import type { Currency } from "./Defs";
import type { PagedResponse } from "./PagedResponse";

export type GetExpensesResponse = PagedResponse<ExpenseDto>;

export type ExpenseDto = {
  id: string;
  shopId?: string;
  shopName?: string;
  totalAmount: number;
  currency: Currency;
  expenseDate: string;
};
