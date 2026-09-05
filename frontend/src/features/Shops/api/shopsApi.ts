import { apiClient } from "../../../api/api";
import type { GetShopsDto } from "../types/GetShopsDto";
import type { SearchShopDto } from "../types/SearchShopDto";

const API_SHOPS = "api/shops";

export async function getShops(): Promise<GetShopsDto[]> {
  return apiClient.get<GetShopsDto[]>(`${API_SHOPS}`);
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
