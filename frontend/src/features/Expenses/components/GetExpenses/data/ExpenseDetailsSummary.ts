import type { Currency } from "../../../../shared/types/Defs";

export type ExpenseDetailsSummary = {
  number: number;
  currency: Currency;
  detailed: number;
  undetailed: number;
};
