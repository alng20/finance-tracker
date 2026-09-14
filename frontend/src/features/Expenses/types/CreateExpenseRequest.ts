import type { AddExpenseDetailData } from "./AddExpenseDetailData";
import type { Currency } from "../../shared/types/Currency";

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

export type CreateExpenseParams = {
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
