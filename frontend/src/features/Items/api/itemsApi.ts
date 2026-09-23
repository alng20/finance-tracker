import { apiClient } from "../../../api/api";
import type { PageRequestData } from "../../shared/types/PageRequestData";
import type { ItemFiltersData } from "../types/data/ItemFiltersData";
import type { SearchItemsDto } from "../types/dto/SearchItemsDto";
import type { CreateItemRequest } from "../types/requests/CreateItemRequest";
import type { UpdateItemRequest } from "../types/requests/UpdateItemRequest";
import type { GetItemsResponse } from "../types/responses/GetItemsResponse";

const API_ITEMS = "api/items";

export async function getItems(
  page: PageRequestData,
  filters?: ItemFiltersData | null,
  reqInit: RequestInit = {},
): Promise<GetItemsResponse> {
  const params = new URLSearchParams({
    page: page.pageNumber.toString(),
    pageSize: page.pageSize.toString(),
  });

  filters?.categories?.forEach((category) => {
    params.append("categoryIds", category.id);
  });

  filters?.units?.forEach((unit) => {
    params.append("units", unit.id);
  });

  const query: string = params.toString();

  return apiClient.get<GetItemsResponse>(
    `${API_ITEMS}${query ? `?${query}` : ""}`,
    reqInit,
  );
}

export async function createItem(request: CreateItemRequest) {
  return apiClient.post<CreateItemRequest>(API_ITEMS, request);
}

export async function updateItem(request: UpdateItemRequest) {
  return apiClient.put<UpdateItemRequest>(API_ITEMS, request);
}

export async function deleteItem(id: string) {
  return apiClient.delete<void>(`${API_ITEMS}/${id}`);
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
