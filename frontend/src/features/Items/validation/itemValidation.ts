import type { ItemCommonData } from "../types/data/ItemData";

export type ItemValidationErrors = {
  name?: string;
  category?: string;
  unit?: string;
};

export function validateItem({
  name,
  categoryId,
  unit,
}: ItemCommonData): ItemValidationErrors {
  const errors: ItemValidationErrors = {};
  const trimmedName = name.trim();

  if (!trimmedName) {
    errors.name = "Name is required.";
  } else if (trimmedName.length > 100) {
    errors.name = "Name must be 100 characters or fewer.";
  }

  if (!categoryId) {
    errors.category = "Category is required.";
  }

  if (!unit) {
    errors.unit = "Unit is required.";
  }

  return errors;
}
