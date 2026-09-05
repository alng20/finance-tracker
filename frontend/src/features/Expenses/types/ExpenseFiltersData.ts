import type { Currency } from "./Defs";

// export type ExpenseFiltersData = {
//   fromDate?: string;
//   toDate?: string;
//   shopIds?: string[];
//   categoryIds?: string[];
//   itemIds?: string[];
//   retailerIds?: string[];
//   currency?: Currency;
//   fromAmount?: number;
//   toAmount?: number;
// };

export type FilterData = {
  id: string;
  name: string;
};

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
