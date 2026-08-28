import { apiClient } from "../../../api/api";
import { makeUrlSearch } from "../../shared/utils";
import type { Shop } from "../types/Shop";

const API_SHOPS = "api/shops";

export async function searchShops(searchString: string): Promise<Shop[]> {
  const response = await apiClient.get(
    `${API_SHOPS}/search?${makeUrlSearch(searchString)}`,
  );
  if (!response.ok) {
    throw new Error("Failed to load items");
  }

  return await response.json();
}
