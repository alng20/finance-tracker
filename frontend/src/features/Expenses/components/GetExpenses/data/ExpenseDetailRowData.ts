import type { Currency } from "../../../../shared/types/Currency";

export type ExpenseDetailRowData = {
  id: string;
  itemName: string;
  categoryId: string;
  categoryName: string;
  unit: string;
  discountPercent: number;
  totalPrice: number;
  currency: Currency;
  quantity: number;
};
