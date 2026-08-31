import type { AddExpenseDetailData } from "./AddExpenseDetailData";
import type { Currency } from "./Defs";

export type CreateExpenseRequest = {
  sharedGroupId: string | null;
  shopId: string | null;
  totalAmount: number;
  currency: Currency;
  expenseDate: string;
  details: CreateExpenseDetailRequest[];
};

export type CreateExpenseDetailRequest = {
  itemId: string;
  totalPrice: number;
  quantity: number;
  discount: Discount | null;
};

export type CreateExpenseData = {
  sharedGroupId: string | null;
  shopId: string | null;
  totalAmount: number;
  currency: Currency;
  expenseDate: string;
  details: AddExpenseDetailData[];
};

export type Discount = {
  value: number;
  type: "Percent" | "Amount";
};
