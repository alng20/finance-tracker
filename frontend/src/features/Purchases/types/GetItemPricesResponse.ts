import type { PagedResponse } from "../../shared/types/PagedResponse";
import type { PriceInfoDto } from "./GetPurchasesResponse";

export type PriceHistoryDto = PriceInfoDto;

export type GetItemPricesResponse = PagedResponse<PriceHistoryDto>;
