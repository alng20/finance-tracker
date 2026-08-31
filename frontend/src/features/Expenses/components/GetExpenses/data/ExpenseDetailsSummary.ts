import type { Currency } from "../../../types/Defs";

export type ExpenseDetailsSummary = {
  number: number;
  currency: Currency;
  detailed: number;
  undetailed: number;
};
