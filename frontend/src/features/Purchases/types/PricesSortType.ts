export const pricesSortTypes = [
  "DateDesc",
  "DateAsc",
  "PriceDesc",
  "PriceAsc",
] as const;

export type PricesSortType = (typeof pricesSortTypes)[number];

export const defaultPricesSortType: PricesSortType = "DateDesc";
