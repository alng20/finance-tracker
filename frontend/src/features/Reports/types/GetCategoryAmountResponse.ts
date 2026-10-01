import type { GetAmountByEntityData } from "./GetAmountByEntityData";

export type GetCategoryAmountResponse = {
  fromDate: string | null;
  toDate: string | null;
  totalAmount: number;
  amounts: CategoryAmount[];
  detailedAmount: number;
  undetailedAmount: number;
};

export type CategoryAmount = GetAmountByEntityData<
  "categoryId",
  "categoryName"
>;
