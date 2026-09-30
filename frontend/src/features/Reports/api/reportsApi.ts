import { apiClient } from "../../../api/api";
import type { Currency } from "../../shared/types/Currency";
import type { GetCategoryAmountResponse } from "../types/GetCategoryAmountResponse";
import type {
  GetGroupedAmountResponse,
  ReportGroupingType,
} from "../types/GetGroupedAmountResponse";
import type { GetRetailerAmountResponse } from "../types/GetRetailerAmountResponse";
import type { GetShopAmountResponse } from "../types/GetShopAmountResponse";
import type { GetTotalAmountResponse } from "../types/GetTotalAmountResponse";

const API_REPORTS = "api/reports";

export async function getTotalAmount(
  currency: Currency,
  fromDate?: string,
  toDate?: string,
  reqInit: RequestInit = {},
): Promise<GetTotalAmountResponse> {
  const params = new URLSearchParams({
    currency,
    ...(fromDate && { fromDate }),
    ...(toDate && { toDate }),
  });

  return apiClient.get<GetTotalAmountResponse>(
    `${API_REPORTS}/total?${params.toString()}`,
    reqInit,
  );
}

export async function getGroupedAmount(
  currency: Currency,
  groupingType: ReportGroupingType,
  fromDate?: string,
  toDate?: string,
  reqInit: RequestInit = {},
): Promise<GetGroupedAmountResponse> {
  const params = new URLSearchParams({
    currency,
    groupingType,
    ...(fromDate && { fromDate }),
    ...(toDate && { toDate }),
  });

  return apiClient.get<GetGroupedAmountResponse>(
    `${API_REPORTS}/grouped?${params.toString()}`,
    reqInit,
  );
}

export async function getCategoryAmount(
  currency: Currency,
  fromDate?: string,
  toDate?: string,
  reqInit: RequestInit = {},
): Promise<GetCategoryAmountResponse> {
  const params = new URLSearchParams({
    currency,
    ...(fromDate && { fromDate }),
    ...(toDate && { toDate }),
  });

  return apiClient.get<GetCategoryAmountResponse>(
    `${API_REPORTS}/by_category?${params.toString()}`,
    reqInit,
  );
}

export async function getShopAmount(
  currency: Currency,
  fromDate?: string,
  toDate?: string,
  reqInit: RequestInit = {},
): Promise<GetShopAmountResponse> {
  const params = new URLSearchParams({
    currency,
    ...(fromDate && { fromDate }),
    ...(toDate && { toDate }),
  });

  return apiClient.get<GetShopAmountResponse>(
    `${API_REPORTS}/by_shop?${params.toString()}`,
    reqInit,
  );
}

export async function getRetailerAmount(
  currency: Currency,
  fromDate?: string,
  toDate?: string,
  reqInit: RequestInit = {},
): Promise<GetRetailerAmountResponse> {
  const params = new URLSearchParams({
    currency,
    ...(fromDate && { fromDate }),
    ...(toDate && { toDate }),
  });

  return apiClient.get<GetRetailerAmountResponse>(
    `${API_REPORTS}/by_retailer?${params.toString()}`,
    reqInit,
  );
}
