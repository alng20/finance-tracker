import type { Item } from "../types/Item";

const API_URL = "http://localhost:5067/api/items";

export async function searchItems(searchString: string): Promise<Item[]> {
  const response = await fetch(
    `${API_URL}?searchString=${encodeURIComponent(searchString)}`,
  );

  if (!response.ok) {
    throw new Error("Failed to load items");
  }

  return await response.json();
}
