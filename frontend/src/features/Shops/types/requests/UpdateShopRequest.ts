export type UpdateShopRequest = {
  id: string;
  name: string;
  retailerId?: string | null;
  country?: string | null;
  city?: string | null;
};
