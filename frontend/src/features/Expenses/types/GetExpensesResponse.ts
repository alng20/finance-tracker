import type { Currency } from "../../shared/types/Defs";
import type { PagedResponse } from "../../shared/types/PagedResponse";

export type GetExpensesResponse = PagedResponse<ExpenseDto>;

export type ExpenseDto = {
  id: string;
  shopId?: string;
  shopName?: string;
  totalAmount: number;
  currency: Currency;
  expenseDate: string;
};
