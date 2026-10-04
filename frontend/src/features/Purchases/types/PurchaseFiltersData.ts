import type { FilterData } from "../../shared/types/FilterData";

export type PurchaseFiltersData = {
  categories?: FilterData[];
  searchString?: string;
  fromDate?: string;
  toDate?: string;
};
