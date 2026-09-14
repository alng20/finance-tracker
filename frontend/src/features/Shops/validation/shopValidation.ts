import type { CreateShopData, UpdateShopData } from "../types/data/ShopData";

export type ShopValidationErrors = {
  name?: string;
  retailer?: string;
  country?: string;
  city?: string;
};

export function validateShop({
  name,
  retailerId,
  country,
  city,
}: CreateShopData | UpdateShopData): ShopValidationErrors {
  const errors: ShopValidationErrors = {};

  if (!name.trim()) {
    errors.name = "Shop name is required.";
  }

  // TODO: Make it not required
  if (!retailerId) {
    errors.retailer = "Retailer is required.";
  }

  // TODO: Make it not required
  if (!country) {
    errors.country = "Country is required.";
  }

  // TODO: Make it not required
  if (!city) {
    errors.city = "City is required.";
  }

  return errors;
}
