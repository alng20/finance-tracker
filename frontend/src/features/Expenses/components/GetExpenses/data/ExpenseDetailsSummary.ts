import type { Currency } from "../../../../shared/types/Currency";

export type ExpenseDetailsSummary = {
  number: number;
  currency: Currency;
  detailed: number;
  undetailed: number;
};
