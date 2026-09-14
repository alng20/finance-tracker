import type { FilterData } from "../../../shared/types/FilterData";

export type ShopFiltersData = {
  retailers?: FilterData[];
  countries?: string[];
  cities?: string[];
};
