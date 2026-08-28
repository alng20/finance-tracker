import { apiClient } from "../../../api/api";
import { makeUrlSearch } from "../../shared/utils";
import type { Item } from "../types/Item";

const API_ITEMS = "api/items";

export async function searchItems(searchString: string): Promise<Item[]> {
  const response = await apiClient.get(
    `${API_ITEMS}?${makeUrlSearch(searchString)}`,
  );

  if (!response.ok) {
    throw new Error("Failed to load items");
  }

  return await response.json();
}
