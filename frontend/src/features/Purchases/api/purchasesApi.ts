import { apiClient } from "../../../api/api";
import { defaultCurrency } from "../../shared/common/consts";
import type { PageRequestData } from "../../shared/types/PageRequestData";
import type { GetItemPricesResponse } from "../types/GetItemPricesResponse";
import type { GetPurchasesResponse } from "../types/GetPurchasesResponse";
import type { PricesSortType } from "../types/PricesSortType";
import type { PurchaseFiltersData } from "../types/PurchaseFiltersData";

const API_PURCHASES = "api/purchases";

export type ItemPricesFilters = {
  fromDate?: string;
  toDate?: string;
  sortType?: PricesSortType;
};

export async function getPurchases(
  page: PageRequestData,
  filters?: PurchaseFiltersData | null,
  reqInit: RequestInit = {},
): Promise<GetPurchasesResponse> {
  const params = new URLSearchParams({
    currency: defaultCurrency,
    page: page.pageNumber.toString(),
    pageSize: page.pageSize.toString(),
  });

  if (filters?.fromDate) {
    params.append("fromDate", filters.fromDate);
  }

  if (filters?.toDate) {
    params.append("toDate", filters.toDate);
  }

  filters?.categories?.forEach((category) => {
    params.append("categoryIds", category.id);
  });

  const searchString = filters?.searchString?.trim();
  if (searchString) {
    params.append("searchString", searchString);
  }

  return apiClient.get<GetPurchasesResponse>(
    `${API_PURCHASES}?${params.toString()}`,
    reqInit,
  );
}

export async function getItemPrices(
  itemId: string,
  page: PageRequestData,
  filters?: ItemPricesFilters | null,
  reqInit: RequestInit = {},
): Promise<GetItemPricesResponse> {
  const params = new URLSearchParams({
    currency: defaultCurrency,
    page: page.pageNumber.toString(),
    pageSize: page.pageSize.toString(),
  });

  if (filters?.fromDate) {
    params.append("fromDate", filters.fromDate);
  }

  if (filters?.toDate) {
    params.append("toDate", filters.toDate);
  }

  if (filters?.sortType) {
    params.append("sortType", filters.sortType);
  }

  return apiClient.get<GetItemPricesResponse>(
    `${API_PURCHASES}/${itemId}?${params.toString()}`,
    reqInit,
  );
}
