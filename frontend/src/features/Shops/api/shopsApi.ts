import { apiClient } from "../../../api/api";
import type { Shop } from "../types/Shop";

const API_SHOPS = "api/shops";

export async function searchShops(searchString: string): Promise<Shop[]> {
  const params = new URLSearchParams({
    searchString,
  });
  return apiClient.get<Shop[]>(`${API_SHOPS}/search?${params.toString()}`);
}
