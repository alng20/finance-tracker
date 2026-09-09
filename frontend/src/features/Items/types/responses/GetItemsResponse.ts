import type { PagedResponse } from "../../../shared/types/PagedResponse";
import type { ItemDto } from "../dto/ItemDto";

export type GetItemsResponse = PagedResponse<ItemDto>;
