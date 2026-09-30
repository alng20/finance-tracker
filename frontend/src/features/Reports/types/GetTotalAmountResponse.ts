import type { Currency } from "../../shared/types/Currency";

export type GetTotalAmountResponse = {
  fromDate: string | null;
  toDate: string | null;
  totalAmount: number;
  currency: Currency;
};
