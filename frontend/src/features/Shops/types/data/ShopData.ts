export interface ShopCommonData {
  name: string;
  retailerId: string;
  country: string;
  city: string;
}

export type CreateShopData = ShopCommonData;

export type UpdateShopData = ShopCommonData & {
  id: string;
};
