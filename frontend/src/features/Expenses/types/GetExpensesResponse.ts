import type { Currency } from "../../shared/types/Currency";
import type { PagedResponse } from "../../shared/types/PagedResponse";

export type GetExpensesResponse = PagedResponse<ExpenseDto, ExpenseMetadata>;

export type ExpenseDto = {
  id: string;
  shopId?: string;
  shopName?: string;
  totalAmount: number;
  currency: Currency;
  expenseDate: string;
};

export type ExpenseMetadata = {
  summaryAmount: number;
  currency: Currency;
};
