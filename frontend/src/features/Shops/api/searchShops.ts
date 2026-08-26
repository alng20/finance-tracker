import type { Shop } from "../types/Shop";

const API_URL = "http://localhost:5067/api/shops";

export async function searchShops(searchString: string): Promise<Shop[]> {
  const response = await fetch(
    `${API_URL}/search?searchString=${encodeURIComponent(searchString)}`,
  );

  if (!response.ok) {
    throw new Error("Failed to load items");
  }

  return await response.json();
}
