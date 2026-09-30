import type { Currency } from "../../shared/types/Currency";

export type ReportFiltersData = {
  fromDate?: string;
  toDate?: string;
  currency: Currency;
};
