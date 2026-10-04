import type { Currency } from "../../shared/types/Currency";
import type { PagedResponse } from "../../shared/types/PagedResponse";

export type PriceInfoDto = {
  price: number;
  currency: Currency;
  date: string;
  shopId?: string | null;
  shopName: string;
};

export type PurchaseDto = {
  id: string;
  name: string;
  categoryId: string;
  categoryName: string;
  unit: string;
  minPrice: PriceInfoDto;
  maxPrice: PriceInfoDto;
};

export type GetPurchasesResponse = PagedResponse<PurchaseDto>;
