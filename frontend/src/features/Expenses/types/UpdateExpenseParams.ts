import type { Currency } from "./Defs";

export type UpdateExpenseParams = {
  sharedGroupId?: string | null;
  shopId: string | null;
  totalAmount: number;
  currency: Currency;
  expenseDate: string;
};
