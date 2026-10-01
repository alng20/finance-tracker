export type GetShopAmountResponse = {
  fromDate: string | null;
  toDate: string | null;
  totalAmount: number;
  amounts: ShopAmount[];
};

export type ShopAmount = {
  shopId: string | null;
  shopName: string;
  totalAmount: number;
};
