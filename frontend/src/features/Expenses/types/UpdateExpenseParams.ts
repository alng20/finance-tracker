import type { Currency } from "../../shared/types/Currency";

export type UpdateExpenseParams = {
  sharedGroupId?: string | null;
  shopId: string | null;
  totalAmount: number;
  currency: Currency;
  expenseDate: string;
};
