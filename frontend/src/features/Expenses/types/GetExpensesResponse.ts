import type { Currency } from "./Defs";

export type GetExpensesResponse = {
  page: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  data: ExpenseDto[];
};

export type ExpenseDto = {
  id: string;
  shopId?: string;
  shopName?: string;
  totalAmount: number;
  currency: Currency;
  expenseDate: string;
};
