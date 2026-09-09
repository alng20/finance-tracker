import type { Currency } from "../../../../shared/types/Defs";

export type ExpenseDetailRowData = {
  id: string;
  itemName: string;
  categoryName: string;
  unit: string;
  discountPercent: number;
  totalPrice: number;
  currency: Currency;
  quantity: number;
};
