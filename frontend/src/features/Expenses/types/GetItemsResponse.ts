import type { PagedResponse } from "./PagedResponse";

export type GetItemsResponse = PagedResponse<ItemDto>;

export type ItemDto = {
  id: string;
  name: string;
  categoryId: string;
  categoryName: string;
  unit: string;
};
