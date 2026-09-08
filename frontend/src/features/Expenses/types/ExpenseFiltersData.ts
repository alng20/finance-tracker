import type { FilterData } from "../../shared/types/FilterData";
import type { Currency } from "./Defs";

export type ExpenseFiltersData = {
  fromDate?: string;
  toDate?: string;
  shops?: FilterData[];
  categories?: FilterData[];
  items?: FilterData[];
  retailers?: FilterData[];
  currency?: Currency;
  fromAmount?: number;
  toAmount?: number;
};
