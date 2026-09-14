import { apiClient } from "../../../api/api";
import type { PageRequestData } from "../../shared/types/PageRequestData";
import type { CreateShopRequest } from "../types/requests/CreateShopRequest";
import type { GetShopsResponse } from "../types/responses/GetShopsResponse";
import type { ShopFiltersData } from "../types/data/ShopFiltersData";
import type { UpdateShopRequest } from "../types/requests/UpdateShopRequest";
import type { SearchShopDto } from "../types/dto/SearchShopDto";

const API_SHOPS = "api/shops";

export async function getShops(
  page: PageRequestData,
  filters?: ShopFiltersData | null,
  reqInit: RequestInit = {},
): Promise<GetShopsResponse> {
  const params = new URLSearchParams({
    page: page.pageNumber.toString(),
    pageSize: page.pageSize.toString(),
  });

  filters?.retailers?.forEach((retailer) => {
    params.append("retailersIds", retailer.id);
  });

  filters?.countries?.forEach((country) => {
    params.append("countries", country);
  });

  filters?.cities?.forEach((city) => {
    params.append("cities", city);
  });

  const query: string = params.toString();

  return apiClient.get<GetShopsResponse>(
    `${API_SHOPS}${query ? `?${query}` : ""}`,
    reqInit,
  );
}

export async function createShop(request: CreateShopRequest) {
  return apiClient.post(API_SHOPS, {
    ...request,
    retailerId: request.retailerId || null,
    country: request.country || null,
    city: request.city || null,
  });
}

export async function updateShop(request: UpdateShopRequest) {
  return apiClient.put(API_SHOPS, {
    ...request,
    retailerId: request.retailerId || null,
    country: request.country || null,
    city: request.city || null,
  });
}

export async function deleteShop(id: string) {
  return apiClient.delete<void>(`${API_SHOPS}/${id}`);
}

export async function searchShops(
  searchString: string,
): Promise<SearchShopDto[]> {
  const params = new URLSearchParams({
    searchString,
  });
  return apiClient.get<SearchShopDto[]>(
    `${API_SHOPS}/search?${params.toString()}`,
  );
}
