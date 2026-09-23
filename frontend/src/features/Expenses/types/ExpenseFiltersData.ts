import type { Currency } from "../../shared/types/Currency";
import type { FilterData } from "../../shared/types/FilterData";
import type { ExpensesSortType } from "./ExpensesSortType";

export type ExpenseFiltersData = {
  sortType?: ExpensesSortType; // TODO: Move to pagination data as generic field?
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
