import { apiClient } from "../../../api/api";
import type { Item } from "../types/Item";

const API_ITEMS = "api/items";

export async function searchItems(searchString: string): Promise<Item[]> {
  const params = new URLSearchParams({
    searchString,
  });
  return apiClient.get<Item[]>(`${API_ITEMS}?${params.toString()}`);
}
