import type { Currency } from "./Defs";

export type GetExpenseByIdResponse = {
  id: string;
  usedId: string;
  sharedGroupId?: string;
  shopId?: string;
  shopName?: string;
  totalAmount: number;
  currency: Currency;
  expenseDate: string;
  details: ExpenseDetailDto[];
  detailedAmount: number;
  undetailedAmount: number;
};

export type ExpenseDetailDto = {
  id: string;
  itemId: string;
  itemName: string;
  categoryName: string;
  unit: string;
  totalPrice: number;
  quantity: number;
  discountPercent: number;
  unitPrice: number;
  unitDiscountPrice?: number;
};
