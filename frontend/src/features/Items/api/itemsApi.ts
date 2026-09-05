import { apiClient } from "../../../api/api";
import type { GetItemsDto } from "../types/GetItemsDto";
import type { SearchItemsDto } from "../types/SearchItemsDto";

const API_ITEMS = "api/items";

export async function getItems(count?: number): Promise<GetItemsDto[]> {
  return apiClient.get<GetItemsDto[]>(
    `${API_ITEMS}${count ? `?count=${count}` : ""}`,
  );
}

export async function searchItems(
  searchString: string,
): Promise<SearchItemsDto[]> {
  const params = new URLSearchParams({
    searchString,
  });
  return apiClient.get<SearchItemsDto[]>(
    `${API_ITEMS}/search?${params.toString()}`,
  );
}
